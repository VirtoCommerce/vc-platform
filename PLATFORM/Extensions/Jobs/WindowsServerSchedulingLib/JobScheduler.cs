using System.Collections.Generic;
using System.Threading;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Quartz.Impl.Matchers;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.PlatformTools;
using VirtoCommerce.Scheduling.LogicalCall;
using VirtoCommerce.Scheduling.LogicalCall.Configuration;
using Timer = System.Timers.Timer;
using TraceContext = VirtoCommerce.Scheduling.LogicalCall.TraceContext;

namespace VirtoCommerce.Scheduling.Windows
{
    public class JobScheduler
    {
        private static ISchedulerFactory _schedulerService;
        private readonly IScheduler _quartzScheduler;
        private readonly bool _isPrimary;
        private readonly Func<Type, IJobActivity> _getJob;
        private readonly ILogger _traceSource;
        private Timer _timer;
        private readonly SchedulerDbContext _schedulerDbTools;

        //System job synchronization interval 1min
        private const int Interval = 1 * 60 * 1000;

		public JobScheduler(bool isPrimary, 
            Func<Type, IJobActivity> getJob, 
            Func<IAppConfigRepository> appConfigRepository, 
            ILogger traceSource)
        {
            // get a quartzScheduler
            _quartzScheduler = SchedulerFactory.GetScheduler();
            _schedulerDbTools = new SchedulerDbContext(appConfigRepository);
		    _traceSource = traceSource;
		    _isPrimary = isPrimary;
		    _getJob = getJob;
        }

        private static ISchedulerFactory SchedulerFactory
        {
            get
            {
                if (_schedulerService == null)
                {
                    var properties = new NameValueCollection
                        {
                            {"quartz.threadPool.type", "Quartz.Simpl.SimpleThreadPool, Quartz"},
                            {"quartz.scheduler.instanceName", "schedulerService"},
                            {"quartz.threadPool.threadPriority", "Normal"},
                            {"quartz.jobStore.misfireThreshold", "60000"}
                        };

                    _schedulerService = new StdSchedulerFactory(properties);
                }
                return _schedulerService;
            }
        }

        public void Start()
        {
            //Immediately synchronize jobs
            SyncJobs(DateTime.Now);

            //Schedule a timer that will periodically synchronize jobs
            if (_timer == null)
            {
                _timer = new Timer(Interval);
                _timer.Elapsed += (sender, args) => SyncJobs(args.SignalTime);
            }
            _timer.Start();

            _traceSource.Info("JobScheduler started");
        }

        public void Stop()
        {
            if (_quartzScheduler.IsStarted)
            {
                _quartzScheduler.Standby();
            }

            if (_timer != null)
            {
                _timer.Stop();
            }

            _traceSource.Info("JobScheduler stopped");
        }

        private void SyncJobs(DateTime runTime)
        {
            try
            {
                //Convert time to UTC
                runTime = runTime.ToUniversalTime();
                var systemJobs = _schedulerDbTools.GetSystemJobs();

                var jobGroups = new List<string>();
                //Get all quartz job group names
                jobGroups.AddRange(_quartzScheduler.GetJobGroupNames());
                //Get all database jobs that are not in quartz
                jobGroups.AddRange(systemJobs.Select(s => s.JobClassType).Where(s => !jobGroups.Contains(s)));

                //Here all jobs will be saved based on their state
                var jobsToAdd = new List<JobKey>();
                var jobsToUpdate = new List<JobKey>();
                var jobsToRemove = new List<JobKey>();

                foreach (var quartzJobName in jobGroups)
                {
                    //Get all system and quartz jobs by current job group name
                    var groupSysJobs = systemJobs.Where(s => s.JobClassType.Equals(quartzJobName)).ToArray();
                    var groupJobKeys = _quartzScheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(quartzJobName));

                    //If there are no jobs for such group name continue
                    if (!groupSysJobs.Any() && !groupJobKeys.Any())
                    {
                        continue;
                    }
                    //If there are no system jobs and some quartz jobs exist for given group name they have to be stopped
                    if (!groupSysJobs.Any() && groupJobKeys.Any())
                    {
                        jobsToRemove.AddRange(groupJobKeys);
                        continue;
                    }
                    //Get available system jobs
                    var availableSysJobs = groupSysJobs.Where(j => j.IsEnabled && 
                        ((j.AllowMultipleInstances) || (!j.AllowMultipleInstances && _isPrimary))).ToArray();

                    //If there are some system jobs avaialble and no such quartz jobs, they all have to be added
                    if (availableSysJobs.Any() && !groupJobKeys.Any())
                    {
                        jobsToAdd.AddRange(availableSysJobs.Select(j => new JobKey(j.SystemJobId, j.JobClassType)));
                        continue;
                    }

                    //Keys that are in quartz jobs but not in available system jobs
                    var remove = groupJobKeys.Where(gk => availableSysJobs.All(j => j.SystemJobId != gk.Name));

                    //Keys that are in system jobs but not in quartz
                    var add = availableSysJobs.Where(j => groupJobKeys.All(gk => j.SystemJobId != gk.Name))
                        .Select(j => new JobKey(j.SystemJobId, j.JobClassType));

                    jobsToRemove.AddRange(remove);
                    jobsToAdd.AddRange(add);

                    //Remaining keys that are in both system and quartz so need to be checked for changes
                    foreach (var updateKey in groupJobKeys.Except(remove))
                    {
                        var job = systemJobs.First(s => s.SystemJobId == updateKey.Name);

                        if (job.IsEnabled &&
                            ((job.AllowMultipleInstances) ||
                             (!job.AllowMultipleInstances && _isPrimary)))
                        {
                            //check if job was modified during current interval
                            if (job.LastModified.HasValue &&
                                job.LastModified.Value.AddMilliseconds(Interval) >= runTime||
                                job.JobParameters.Any(x => x.LastModified.HasValue &&
                                    x.LastModified.Value.AddMilliseconds(Interval) >= runTime))
                            {
                                jobsToUpdate.Add(updateKey);
                            }
                        }
                        else
                        {
                            jobsToRemove.Add(updateKey);
                        }
                    }

                }

                //Remove jobs
                foreach (var jobKey in jobsToRemove.Where(jobKey => _quartzScheduler.CheckExists(jobKey)))
                {
                    _quartzScheduler.DeleteJob(jobKey);
                }

                //Add and Update Jobs
                foreach (var jobKey in jobsToAdd.Union(jobsToUpdate))
                {
                    AddJob(_quartzScheduler, systemJobs.First(x => x.SystemJobId == jobKey.Name), _getJob);
                }

                if (!_quartzScheduler.IsStarted)
                {
                    _quartzScheduler.Start();
                }
            }
            catch (Exception ex)
            {
                _traceSource.Error(ex.ToString());

                //If database does not exist
                if (ex is ConfigurationErrorsException)
                {
                    Stop();
                }
            }
        }

        private void AddJob(IScheduler scheduler, SystemJob jobItem, Func<Type, IJobActivity> getJob)
        {
            var jobKey = new JobKey(jobItem.SystemJobId, jobItem.JobClassType);
            var type = Type.GetType(jobItem.JobClassType, true);

            // Define the Job to be scheduled
            var quartzJobDetail = JobBuilder.Create(typeof(QuartzJob))
                .WithIdentity(jobKey)
                .RequestRecovery()
                .Build();

            Func<IJobActivity> jobActivityConstructor = () => getJob(type);

            Func<DateTime, Action<string>> getAudit = startDateTime =>
                message => _schedulerDbTools.CreateSystemJobLogEntry(
                jobItem.SystemJobId, startDateTime, DateTime.Now, message,
                Thread.CurrentThread.Name, null, jobItem.AllowMultipleInstances);

            var section = ConfigurationManager.GetSection("traceContextConfiguration") ?? new TraceContextConfigurationSection();
            ITraceContextConfigurator config = (TraceContextConfigurationSection)section;
            var contextName = new ContextName(type.FullName, "");
            var configuration = config.GetDefault(contextName.Service, contextName.Method);
            var traceSource = new TraceSource("VirtoCommerce.ScheduleService.Trace");
            var traceContext = new TraceContext(configuration, contextName, Guid.NewGuid(), traceSource);

            quartzJobDetail.JobDataMap.Add("realization", jobActivityConstructor);
            quartzJobDetail.JobDataMap.Add("getAudit", getAudit);
            quartzJobDetail.JobDataMap.Add("context", traceContext);
            quartzJobDetail.JobDataMap.Add("parameters", jobItem.JobParameters.ToDictionary(pk => pk.Name.ToLowerInvariant(), pv => pv.Value));

            // Associate a trigger with the Job
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobItem.SystemJobId, jobItem.JobClassType)
                .WithSimpleSchedule(x =>
                {
                    x.WithInterval(TimeSpan.FromSeconds(jobItem.Period));
                    x.RepeatForever();
                })
                .StartAt(DateTime.UtcNow)
                .WithPriority(jobItem.Priority)
                .Build();

            // Validate that the job doesn't already exists
            if (scheduler.CheckExists(jobKey))
            {
                scheduler.DeleteJob(jobKey);
            }
            scheduler.ScheduleJob(quartzJobDetail, trigger);
        }
    }
}

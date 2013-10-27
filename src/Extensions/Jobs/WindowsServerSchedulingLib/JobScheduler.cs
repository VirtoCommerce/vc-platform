using System.Collections.Generic;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Quartz.Impl.Matchers;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.PlatformTools;
using VirtoCommerce.Scheduling.LogicalCall;
using VirtoCommerce.Scheduling.LogicalCall.Configuration;
using TraceContext = VirtoCommerce.Scheduling.LogicalCall.TraceContext;

namespace VirtoCommerce.Scheduling.Windows
{
    public class JobScheduler
    {
        private readonly SchedulerChecker _schedulerChecker;
        private static ISchedulerFactory _schedulerService;

		public JobScheduler(bool isPrimary, Func<Type, IJobActivity> getJob, Func<IAppConfigRepository> appConfigRepository, ILogger traceSource)
        {
            // get a quartzScheduler
            var quartzScheduler = SchedulerFactory.GetScheduler();
			traceSource.Info("JobScheduler created");
            _schedulerChecker = new SchedulerChecker(quartzScheduler, isPrimary, getJob, appConfigRepository, traceSource);
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
            _schedulerChecker.Start();
        }

        public void Stop()
        {
            _schedulerChecker.Stop();
        }

        public class SchedulerChecker // In App_Code folder
        {
            private readonly IScheduler _quartzScheduler;
            private readonly bool _isPrimary;
            private readonly Func<Type, IJobActivity> _getJob;
            private readonly Func<IAppConfigRepository> _appConfigRepository;
            private readonly ILogger _traceSource;
            private Timer _timer;

            //System job synchronization interval 1min
            private const int Interval = 1*60*1000;

            public SchedulerChecker(IScheduler jobScheduler, bool isPrimary, Func<Type, IJobActivity> getJob, Func<IAppConfigRepository> appConfigRepository, ILogger traceSource)
            {
                _quartzScheduler = jobScheduler;
                _isPrimary = isPrimary;
                _getJob = getJob;
                _appConfigRepository = appConfigRepository;
                _traceSource = traceSource;
            }

            public void Start()
            {
                SyncJobs(DateTime.Now);
                _timer = new Timer(Interval);
                _timer.Elapsed += (sender, args) => SyncJobs(args.SignalTime);
                _timer.Start();
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
            }

            private void SyncJobs(DateTime runTime)
            {
                try
                {
                    var schedulerDbTools = new SchedulerDbContext(_appConfigRepository);
                    var systemJobs = schedulerDbTools.GetSystemJobs();

                    var jobGroups = new List<string>();
                    jobGroups.AddRange(_quartzScheduler.GetJobGroupNames());
                    jobGroups.AddRange(systemJobs.Select(s=>s.JobClassType).Where(s=>!jobGroups.Contains(s)));

                    var jobsToAdd = new List<JobKey>();
                    var jobsToUpdate = new List<JobKey>();
                    var jobsToRemove = new List<JobKey>();

                    foreach (var quartzJobName in jobGroups)
                    {
                        var groupSysJobs = systemJobs.Where(s => s.JobClassType.Equals(quartzJobName)).ToArray();
                        var groupJobKeys = _quartzScheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(quartzJobName));

                        if (!groupSysJobs.Any() && !groupJobKeys.Any())
                        {
                            continue;
                        }

                        if (!groupSysJobs.Any() && groupJobKeys.Any())
                        {
                            jobsToRemove.AddRange(groupJobKeys);
                            continue;
                        }

                        if (groupSysJobs.Any() && !groupJobKeys.Any())
                        {
                            jobsToAdd.AddRange(groupSysJobs.Where(j=> j.IsEnabled 
                                && ((j.AllowMultipleInstances) || (!j.AllowMultipleInstances && _isPrimary)))
                                .Select(j => new JobKey(j.SystemJobId, j.JobClassType)));
                            continue;
                        }

                        //Keys that are in quartz jobs but not in system jobs
                        var remove = groupJobKeys.Where(gk => groupSysJobs.All(j => j.SystemJobId != gk.Name));

                        //Keys that are in system jobs but not in quartz
                        var add = groupSysJobs.Where(j => j.IsEnabled
                            && ((j.AllowMultipleInstances) || (!j.AllowMultipleInstances && _isPrimary))
                            && groupJobKeys.All(gk => j.SystemJobId != gk.Name))
                            .Select(j => new JobKey(j.SystemJobId, j.JobClassType));

                        jobsToRemove.AddRange(remove);
                        jobsToAdd.AddRange(add);

                        //Remaining keys that are in both system and quartz
                        foreach (var updateKey in groupJobKeys.Except(remove))
                        {
                            var job = systemJobs.First(s => s.SystemJobId == updateKey.Name);

                            if (job.IsEnabled &&
                                ((job.AllowMultipleInstances) ||
                                 (!job.AllowMultipleInstances && _isPrimary)))
                            {
                                //check if job was modified during current interval
                                if (job.LastModified.HasValue &&
                                    job.LastModified.Value.AddMilliseconds(Interval) >= runTime ||
                                    job.JobParameters.Any(x => x.LastModified.HasValue && 
                                        x.LastModified.Value.AddMilliseconds(Interval) >= runTime.ToUniversalTime()))
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
                    throw;
                }
            }

            private static void AddJob(IScheduler scheduler, SystemJob jobItem, Func<Type, IJobActivity> getJob)
            {
                //Trace.TraceInformation(jobItem.JobClassType);
                var type = Type.GetType(jobItem.JobClassType, true);
                // Define the Job to be scheduled
                var quartzJobDetail = JobBuilder.Create(typeof(QuartzJob))
                    .WithIdentity(jobItem.SystemJobId, jobItem.JobClassType)
                    .RequestRecovery()
                    .Build();

                Func<IJobActivity> jobActivityConstructor = () => getJob(type);

                var section = ConfigurationManager.GetSection("traceContextConfiguration") ?? new TraceContextConfigurationSection();
                ITraceContextConfigurator config = (TraceContextConfigurationSection)section;
                var contextName = new ContextName(type.FullName, "");
                var configuration = config.GetDefault(contextName.Service, contextName.Method);
                var traceSource = new TraceSource("VirtoCommerce.ScheduleService.Trace");
                var traceContext = new TraceContext(configuration, contextName, Guid.NewGuid(), traceSource);

                quartzJobDetail.JobDataMap.Add("realization", jobActivityConstructor);
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
                var jobKey = new JobKey(jobItem.SystemJobId, jobItem.JobClassType);
                if (scheduler.CheckExists(jobKey))
                {
                    scheduler.DeleteJob(jobKey);
                }
                scheduler.ScheduleJob(quartzJobDetail, trigger);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Scheduling
{
    /// <summary>
    /// Class SchedulerDbContext.
    /// </summary>
    public class SchedulerDbContext : ISchedulerDbContext
    {
        /// <summary>
        /// The _repository factory
        /// </summary>
        private readonly Func<IAppConfigRepository> _repositoryFactory;
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDbContext"/> class.
        /// </summary>
        /// <param name="appConfigRepositoryFactory">The application configuration repository factory.</param>
        public SchedulerDbContext(Func<IAppConfigRepository> appConfigRepositoryFactory)
        {
            _repositoryFactory = appConfigRepositoryFactory;
        }
        /// <summary>
        /// Gets the system jobs.
        /// </summary>
        /// <returns>List{SystemJob}.</returns>
        public List<SystemJob> GetSystemJobs()
        {
            var systemJobs = _repositoryFactory().SystemJobs.Expand(sj => sj.JobParameters).ToList();
            return systemJobs;
        }

        /// <summary>
        /// Gets the singleton tasks with alarms.
        /// </summary>
        /// <returns>List{TaskSchedule}.</returns>
        public List<TaskSchedule> GetSingletonTasksWithAlarms()
        {
            List<TaskSchedule> tasks;
            var dateTime = DateTime.UtcNow;
            var repository = _repositoryFactory();

            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (
                    var transaction = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var q1 = from ts in repository.TaskSchedules
                         join sj in repository.SystemJobs on ts.SystemJobId equals sj.SystemJobId
                         where !sj.IsEnabled || ts.Frequency <= 0
                         select ts;
                var tasksForRemove = q1.ToList();
                tasksForRemove.ForEach(repository.Remove);

                var q2 = from ts in repository.TaskSchedules
                         join sj in repository.SystemJobs on ts.SystemJobId equals sj.SystemJobId
                         where ts.NextScheduledStartTime < dateTime
                         select ts;
                tasks = q2.ToList();

                repository.UnitOfWork.Commit();
                transaction.Complete();
            }

            return tasks;
        }

        /// <summary>
        /// Periods to frequency.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns>System.Int32.</returns>
        public static int PeriodToFrequency(int period)
        {
            if (period == 0)
                return 0;
            var f = period / 60;
            return period / 60 == 0 ? 1 : f;
        }

        /// <summary>
        /// Determines whether the specified system job identifier is disabled.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
        /// <param name="prevMultipleInstances">if set to <c>true</c> [previous multiple instances].</param>
        /// <returns><c>true</c> if the specified system job identifier is disabled; otherwise, <c>false</c>.</returns>
        public bool IsDisabled(
            string systemJobId, bool prevMultipleInstances)
        {
            bool value = false;
            var repository = _repositoryFactory();

            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var job = repository.SystemJobs.FirstOrDefault(sj => sj.SystemJobId == systemJobId);

                if (job == null
                     || !job.IsEnabled
                     || job.Period <= 0
                     || prevMultipleInstances != job.AllowMultipleInstances
                    )
                {
                    value = true;
                }
                repository.UnitOfWork.Commit();
                transaction.Complete();
            }
            return value;
        }

        /// <summary>
        /// Creates the system job log entry.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="message">The message.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="taskScheduleId">The task schedule identifier.</param>
        /// <param name="multipleInstance">if set to <c>true</c> [multiple instance].</param>
        public void CreateSystemJobLogEntry(string systemJobId, DateTime startTime,DateTime endTime, 
            string message, string instance, string taskScheduleId, bool multipleInstance)
        {
            var repository = _repositoryFactory();
            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var systemJobLogEntry = new SystemJobLogEntry
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    Message = message,
                    SystemJobId = systemJobId,
                    Instance = instance,
                    MultipleInstance = multipleInstance,
                    TaskScheduleId = taskScheduleId
                };
                repository.Add(systemJobLogEntry);
                repository.UnitOfWork.Commit();
                transaction.Complete();
            }
        }

        // this should return new jobs and create tasks for them 
        /// <summary>
        /// Prepares the task schedule.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
        /// <param name="allowMultipleInstances">if set to <c>true</c> [allow multiple instances].</param>
        public void PrepareTaskSchedule(string systemJobId, bool allowMultipleInstances)
        {
            var repository = _repositoryFactory();

            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var job = repository.SystemJobs.FirstOrDefault(sj => sj.SystemJobId == systemJobId);
                if (job != null && job.IsEnabled && job.Period > 0 && job.AllowMultipleInstances == allowMultipleInstances)
                {
                    var taskSchedule = repository.TaskSchedules.SingleOrDefault(ts => ts.SystemJobId == systemJobId);
                    if (taskSchedule == null)
                    {
                        var startedDateTime = DateTime.UtcNow;
                        var originalFrequency = PeriodToFrequency(job.Period);
                        var newTaskSchedule = new TaskSchedule
                        {
                            Frequency = originalFrequency,
                            NextScheduledStartTime = startedDateTime.AddMinutes(originalFrequency),
                            SystemJobId = systemJobId
                        };
                        repository.Add(newTaskSchedule);
                    }
                    repository.UnitOfWork.Commit();
                }
                transaction.Complete();
            }
        }


        /// <summary>
        /// Updates the task schedule.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
        public void UpdateTaskSchedule(string systemJobId)
        {
            var repository = _repositoryFactory();

            using (SqlDbConfiguration.ExecutionStrategySuspension)
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var job = repository.SystemJobs.FirstOrDefault(sj => sj.SystemJobId == systemJobId);
                var task = repository.TaskSchedules.SingleOrDefault(ts => ts.SystemJobId == systemJobId);
                if (task != null)
                {
                    repository.Remove(task);
                }
                if (job != null && job.IsEnabled && job.Period > 0)
                {
                    var startedDateTime = DateTime.UtcNow;
                    var originalFrequency = PeriodToFrequency(job.Period);
                    var newTaskSchedule = new TaskSchedule
                    {
                        Frequency = originalFrequency,
                        NextScheduledStartTime = startedDateTime.AddMinutes(originalFrequency),
                        SystemJobId = systemJobId
                    };
                    repository.Add(newTaskSchedule);
                }
                repository.UnitOfWork.Commit();
                transaction.Complete();
            }
        }
    }
}
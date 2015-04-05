using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.Scheduling
{
    /// <summary>
    /// Interface ISchedulerDbContext
    /// </summary>
	public interface ISchedulerDbContext
	{
        /// <summary>
        /// Updates the task schedule.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
		void UpdateTaskSchedule(string systemJobId);

        /// <summary>
        /// Gets the system jobs.
        /// </summary>
        /// <returns>List{SystemJob}.</returns>
		List<SystemJob> GetSystemJobs();
        /// <summary>
        /// Gets the singleton tasks with alarms.
        /// </summary>
        /// <returns>List{TaskSchedule}.</returns>
		List<TaskSchedule> GetSingletonTasksWithAlarms();

        /// <summary>
        /// Determines whether the specified system job identifier is disabled.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
        /// <param name="prevMultipleInstances">if set to <c>true</c> [previous multiple instances].</param>
        /// <returns><c>true</c> if the specified system job identifier is disabled; otherwise, <c>false</c>.</returns>
		bool IsDisabled(string systemJobId, bool prevMultipleInstances);

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
		void CreateSystemJobLogEntry(string systemJobId, DateTime startTime, DateTime endTime, 
            string message,string instance, string taskScheduleId, bool multipleInstance);

        /// <summary>
        /// Prepares the task schedule.
        /// </summary>
        /// <param name="systemJobId">The system job identifier.</param>
        /// <param name="allowMultipleInstances">if set to <c>true</c> [allow multiple instances].</param>
		void PrepareTaskSchedule(string systemJobId,bool allowMultipleInstances);
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Scheduling;

namespace FunctionalTests.Azure
{
	public class TestISchedulerDbContext : ISchedulerDbContext
	{
		public readonly List<TaskSchedule> TaskSchedules = new List<TaskSchedule>();
		public readonly List<SystemJob> SystemJobs = new List<SystemJob>();

		public class TestJob : IJobActivity
		{
			public void Execute(IJobContext context)
			{

			}
		}

		public bool IsDisabled(string systemJobId, bool prevMultipleInstances)
		{
		   lock(keylock)	
		   {
						
					bool value = false;
					var job = SystemJobs.FirstOrDefault(sj => sj.SystemJobId == systemJobId);

					if (job == null
					    || !job.IsEnabled
					    || job.Period <= 0
					    || prevMultipleInstances != job.AllowMultipleInstances
						)
					{
						value = true;
					}
					return value;
			}
		} 

		public TestISchedulerDbContext()
		{
			SystemJobs.Add(new SystemJob
			{
				SystemJobId = "{1E2F0E13-5F90-45FB-B43B-76D38D5684D8}",
				AllowMultipleInstances = true,
				JobClassType = typeof(TestJob).AssemblyQualifiedName,
				Period = 60,
				IsEnabled = true
			});
			SystemJobs.Add(new SystemJob
			{
				SystemJobId = "{3C929690-6F76-4E55-AEE6-8ADFCF7E1170}",
				AllowMultipleInstances = false,
				JobClassType = typeof(TestJob).AssemblyQualifiedName,
				Period = 60,
				IsEnabled = true
			});
			SystemJobs.Add(new SystemJob
			{
				SystemJobId = "{1C5340E9-C7E7-46F7-B1CF-25E1DB15CAB5}",
				AllowMultipleInstances = false,
				JobClassType = typeof(TestJob).AssemblyQualifiedName,
				Period = 60,
				IsEnabled = true
			});
		}

		public List<SystemJob> GetSystemJobs()
		{
			return SystemJobs;
		}

		public void UpdateTaskSchedule(string systemJobId)
		{
			lock (keylock)
			{
				var job = SystemJobs.FirstOrDefault(sj => sj.SystemJobId == systemJobId);
				var task = TaskSchedules.SingleOrDefault(ts => ts.SystemJobId == systemJobId);
				if (task != null)
				{
					TaskSchedules.Remove(task);
				}
				if (job != null && job.IsEnabled && job.Period > 0)
				{
					var startedDateTime = DateTime.UtcNow;
					var originalFrequency = SchedulerDbContext.PeriodToFrequency(job.Period);
					var newTaskSchedule = new TaskSchedule
					{
						Frequency = originalFrequency,
						NextScheduledStartTime = startedDateTime.AddMinutes(originalFrequency),
						SystemJobId = systemJobId
					};
					TaskSchedules.Add(newTaskSchedule);
				}
			}
		}

		private string keylock = "kokoko";
		public List<TaskSchedule> GetSingletonTasksWithAlarms()
		{
			lock (keylock)
			{
				var dateTime = DateTime.UtcNow;
				var q2 = from ts in TaskSchedules
					where ts.NextScheduledStartTime < dateTime
					select ts;
				return q2.ToList();
			}
		}

		public void PrepareTaskSchedule(string systemJobId, bool allowMultipleInstances)
		{
			lock (keylock)
			{
				var job = SystemJobs.FirstOrDefault(sj => sj.SystemJobId == systemJobId);
				var taskSchedule = TaskSchedules.SingleOrDefault(ts => ts.SystemJobId == systemJobId);
				if (taskSchedule == null)
				{
					var startedDateTime = DateTime.UtcNow;
					var originalFrequency = SchedulerDbContext.PeriodToFrequency(job.Period);
					var newTaskSchedule = new TaskSchedule
					{
						Frequency = originalFrequency,
						NextScheduledStartTime = startedDateTime.AddMinutes(originalFrequency),
						SystemJobId = systemJobId
					};
					TaskSchedules.Add(newTaskSchedule);
				}
			}
		}



		public void CreateSystemJobLogEntry(string systemJobId, DateTime startTime, DateTime endTime, string message, string instance,
			string taskScheduleId, bool multipleInstance)
		{
			if (message == null)
				Count++;
			else
				Error++;
		}

		public int Count { get; set; }
		public int Error { get; set; }

	}
}
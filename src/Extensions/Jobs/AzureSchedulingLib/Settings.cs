using System;

namespace VirtoCommerce.Scheduling.Azure
{
    public class Settings
    {
	    private readonly string _suffix;
        public const int RunJobsManagerWakeupFrequency = 30 * 1000;
        public const int QueueListenerWakeupFrequency  = 30 * 1000;
		public const int RunSchedulerWakeupFrequency   = 30 * 1000;

	    public Settings(string suffix)
	    {
		    _suffix = suffix;
	    }

		public Settings():this("")
		{
		}

	    public string GetContainerName()
		{
			return @"taskscheduler-container" + (_suffix == "" ? "" : "-") + _suffix;
		}

	    public string GetQueueName(string id)
		{
			return String.Format("taskscheduler-queue{0}-{1}",(_suffix == "" ? "" : "-") + _suffix, NormalizeName(id));
		}

		public static string NormalizeName(string id)
		{
			return Guid.Parse(id).ToString("N").ToLower();
		}

	    public static string GetLeaseId(string id)
	    {
			return NormalizeName(id);
	    }

	    public string GetBlockBlobName()
	    {
			return "taskscheduler-blockBlob" + (_suffix == "" ? "" : "-") + _suffix;
	    }
    }
}


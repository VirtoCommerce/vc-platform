using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Web.BackgroundJobs
{
	public class SendNotificationsJobsScheduler
	{
        public void ScheduleJobs()
        {
			RecurringJob.AddOrUpdate<SendNotificationsJobs>("SendNotificationsJob", x => x.Process(), "*/1 * * * *");
        }
	}
}
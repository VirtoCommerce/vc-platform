using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.BackgroundJobs
{
	public class SendNotificationsJobs
	{
		private readonly INotificationManager _notificationManager;

		private readonly int _take;

		public SendNotificationsJobs(INotificationManager notificationManager, ISettingsManager settingsManager)
        {
			if (notificationManager != null)
			{
				_notificationManager = notificationManager;
			}

			if(settingsManager != null)
			{
				var takeSetting = settingsManager.GetSettingByName("VirtoCommerce.Platform.Notifications.SendingJob.TakeCount");
				_take = Convert.ToInt32(takeSetting.Value);
			}
        }

        [DisableConcurrentExecution(60 * 60 * 24)]
        public void Process()
        {
			var criteria = new SearchNotificationCriteria() { IsActive = true, Take = _take };

			var result = _notificationManager.SearchNotifications(criteria);
			if(result != null && result.Notifications != null && result.Notifications.Count > 0)
			{
				foreach(var notification in result.Notifications)
				{
					notification.AttemptCount++;
					var sendResult = notification.SendNotification();
					if(sendResult.IsSuccess)
					{
						notification.IsActive = false;
						notification.IsSuccessSend = true;
						notification.SentDate = DateTime.UtcNow;
					}
					else
					{
						if(notification.AttemptCount >= notification.MaxAttemptCount)
						{
							notification.IsActive = false;
						}

						notification.LastFailAttemptDate = DateTime.UtcNow;
						notification.LastFailAttemptMessage = sendResult.ErrorMessage;
					}

					_notificationManager.UpdateNotification(notification);
				}
			}
        }
	}
}
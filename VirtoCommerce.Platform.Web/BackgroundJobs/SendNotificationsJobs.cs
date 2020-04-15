﻿using Hangfire;
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
		private readonly int _sendingBatchSize;
        private readonly int _settingsRepeatInterval;

		public SendNotificationsJobs(INotificationManager notificationManager, ISettingsManager settingsManager)
        {
			_notificationManager = notificationManager;
	        _sendingBatchSize = settingsManager.GetValue("VirtoCommerce.Platform.Notifications.SendingJob.TakeCount", 20);
            _settingsRepeatInterval = settingsManager.GetValue("VirtoCommerce.Platform.Notifications.SendingJob.RepeatInterval", 5*60);
        }

        [DisableConcurrentExecution(60 * 60 * 24)]
        public void Process()
        {
			var criteria = new SearchNotificationCriteria
            {
                IsActive = true,
                Take = _sendingBatchSize,
                RepeatMinutesIntervalForFail = _settingsRepeatInterval
            };

			var result = _notificationManager.SearchNotifications(criteria);
			if(result != null && result.Notifications != null && result.Notifications.Count > 0)
			{
				foreach(var notification in result.Notifications)
				{
					notification.AttemptCount++;
                    SendNotificationResult sendResult = null;
                    try
                    {
                        sendResult = notification.SendNotification();
                    }
                    catch (Exception ex)
                    {
                        sendResult = new SendNotificationResult
                        {
                            IsSuccess = false,
                            ErrorMessage = ex.ToString()
                        };
                    }

                    if (!sendResult.IsSuccess)
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

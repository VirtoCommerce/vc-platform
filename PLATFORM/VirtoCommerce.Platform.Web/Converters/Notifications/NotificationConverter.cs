using System;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Platform.Web.Model.Notifications;
using coreModels = VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Web.Converters.Notifications
{
	public static class NotificationConverter
	{
		public static webModels.Notification ToWebModel(this coreModels.Notification notification)
		{
			webModels.Notification retVal = new webModels.Notification();

			retVal.InjectFrom(notification);

			retVal.IsEmail = notification.NotificationSendingGateway is IEmailNotificationSendingGateway;
			retVal.IsSms = notification.NotificationSendingGateway is ISmsNotificationSendingGateway;

			return retVal;
		}

		public static webModels.NotificationTemplate ToWebModel(this coreModels.NotificationTemplate notificationTemplate)
		{
			webModels.NotificationTemplate retVal = new webModels.NotificationTemplate();

			retVal.InjectFrom(notificationTemplate);

			return retVal;
		}

		public static coreModels.NotificationTemplate ToCoreModel(this webModels.NotificationTemplate notificationTemplate)
		{
			coreModels.NotificationTemplate retVal = new coreModels.NotificationTemplate();

			retVal.InjectFrom(notificationTemplate);

			return retVal;
		}

		public static webModels.NotificationParameter ToWebModel(this coreModels.NotificationParameter notificationParameter)
		{
			webModels.NotificationParameter retVal = new webModels.NotificationParameter();

			retVal.InjectFrom(notificationParameter);

			return retVal;
		}
	}
}
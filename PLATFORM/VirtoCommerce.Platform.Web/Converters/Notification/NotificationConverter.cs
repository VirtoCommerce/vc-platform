using System;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.Platform.Web.Model.Notification;
using coreModels = VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Web.Converters.Notification
{
	public static class NotificationConverter
	{
		public static webModels.Notification ToWebModel(this coreModels.Notification notification)
		{
			webModels.Notification retVal = new webModels.Notification();

			retVal.InjectFrom(notification);

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
	}
}
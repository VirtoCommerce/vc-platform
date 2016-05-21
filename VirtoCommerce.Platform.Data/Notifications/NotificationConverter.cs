using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.Platform.Data.Notifications
{
	public static class NotificationConverter
	{
		public static NotificationEntity ToDataModel(this Core.Notifications.Notification notification)
		{
			NotificationEntity retVal = new NotificationEntity();

			retVal.InjectFrom(notification);

			retVal.SendingGateway = notification.NotificationSendingGateway.GetType().Name;

			return retVal;
		}

		public static NotificationTemplateEntity ToDataModel(this Core.Notifications.NotificationTemplate notificationTemplate)
		{
			NotificationTemplateEntity retVal = new NotificationTemplateEntity();

			retVal.InjectFrom(notificationTemplate);

			return retVal;
		}

		public static Core.Notifications.NotificationTemplate ToCoreModel(this NotificationTemplateEntity notificationTemplate)
		{
			Core.Notifications.NotificationTemplate retVal = new Core.Notifications.NotificationTemplate();

			retVal.InjectFrom(notificationTemplate);

			return retVal;
		}

		public static void Patch(this NotificationTemplateEntity source, NotificationTemplateEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<NotificationTemplateEntity>(x => x.Subject, x => x.Body, x => x.IsDefault);
			target.InjectFrom(patchInjection, source);
		}
	}
}

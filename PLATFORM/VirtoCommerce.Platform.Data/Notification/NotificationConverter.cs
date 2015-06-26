using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.Platform.Data.Notification
{
	public static class NotificationConverter
	{
		public static NotificationEntity ToDataModel(this Core.Notification.Notification notification)
		{
			NotificationEntity retVal = new NotificationEntity();

			retVal.InjectFrom(notification);

			retVal.SendingGateway = notification.NotificationSendingGateway.GetType().Name;

			return retVal;
		}

		public static NotificationTemplateEntity ToDataModel(this Core.Notification.NotificationTemplate notificationTemplate)
		{
			NotificationTemplateEntity retVal = new NotificationTemplateEntity();

			retVal.InjectFrom(notificationTemplate);

			return retVal;
		}

		public static Core.Notification.NotificationTemplate ToCoreModel(this NotificationTemplateEntity notificationTemplate)
		{
			Core.Notification.NotificationTemplate retVal = new Core.Notification.NotificationTemplate();

			retVal.InjectFrom(notificationTemplate);

			return retVal;
		}

		public static void Patch(this NotificationTemplateEntity source, NotificationTemplateEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<NotificationTemplateEntity>(x => x.DisplayName, x => x.Subject, x => x.Body);
			target.InjectFrom(patchInjection, source);
		}
	}
}

using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public static class NotificationConverter
    {
        public static NotificationEntity ToDataModel(this Core.Notifications.Notification notification)
        {
            var retVal = new NotificationEntity();

            retVal.InjectFrom(notification);

            retVal.SendingGateway = notification.NotificationSendingGateway.GetType().Name;

            if (notification is EmailNotification emailNotification)
            {
                retVal.Сс = !emailNotification.CC.IsNullOrEmpty() ? string.Join(",", emailNotification.CC) : null;
                retVal.Bcс = !emailNotification.Bcc.IsNullOrEmpty() ? string.Join(",", emailNotification.Bcc) : null;
            }

            return retVal;
        }

        public static NotificationTemplateEntity ToDataModel(this Core.Notifications.NotificationTemplate notificationTemplate)
        {
            var retVal = new NotificationTemplateEntity();

            retVal.InjectFrom(notificationTemplate);

            return retVal;
        }

        public static Core.Notifications.NotificationTemplate ToCoreModel(this NotificationTemplateEntity notificationTemplate)
        {
            var retVal = new Core.Notifications.NotificationTemplate();

            retVal.InjectFrom(notificationTemplate);

            return retVal;
        }

        public static void Patch(this NotificationTemplateEntity source, NotificationTemplateEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<NotificationTemplateEntity>(x => x.Sender, x => x.Recipient, x => x.Subject, x => x.Body, x => x.Language, x => x.IsDefault);
            target.InjectFrom(patchInjection, source);
        }
    }
}

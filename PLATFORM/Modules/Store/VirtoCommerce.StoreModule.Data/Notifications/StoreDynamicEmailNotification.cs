using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.StoreModule.Data.Notifications
{
    public class StoreDynamicEmailNotification : EmailNotification
    {
        public StoreDynamicEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }

        [NotificationParameter("Type of form that initialize sending notification")]
        public string FormType { get; set; }

        [NotificationParameter("Form fields of notification")]
        public IDictionary<string, string> Fields { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public IDictionary Fields { get; set; }
    }


}

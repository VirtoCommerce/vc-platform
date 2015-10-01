using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.MerchandisingModule.Web.Model.Notificaitons
{
    public class DynamicMerchandisingNotification : EmailNotification
    {
        public DynamicMerchandisingNotification(IEmailNotificationSendingGateway gateway) : base (gateway) { }

        [NotificationParameter("Type of form that initialize sending notification")]
        public string FormType { get; set; }

        [NotificationParameter("Form fields of notification")]
        public IDictionary Fields { get; set; }
    }
}
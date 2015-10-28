using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notifications
{
    public class NewOrderStatusEmailNotification : EmailNotification
    {
        public NewOrderStatusEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }

        [NotificationParameter("Readable order number")]
        public string OrderNumber { get; set; }

        [NotificationParameter("Old order status")]
        public string OldStatus { get; set; }

        [NotificationParameter("New order status")]
        public string NewStatus { get; set; }
    }
}

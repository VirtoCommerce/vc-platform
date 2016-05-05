using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notifications
{
    public class NewOrderStatusEmailNotification : OrderEmailNotificationBase
    {
        public NewOrderStatusEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }

      
        [NotificationParameter("Old order status")]
        public string OldStatus { get; set; }

        [NotificationParameter("New order status")]
        public string NewStatus { get; set; }
    }
}

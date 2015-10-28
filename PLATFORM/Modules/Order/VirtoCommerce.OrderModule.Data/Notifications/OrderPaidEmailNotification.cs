using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notifications
{
    public class OrderPaidEmailNotification : EmailNotification
    {
        public OrderPaidEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway){ }

        [NotificationParameter("Readable order number")]
        public string OrderNumber { get; set; }

        [NotificationParameter("Paid date")]
        public DateTime PaidDate { get; set; }

        [NotificationParameter("Full order paid amount")]
        public decimal FullPrice { get; set; }

        [NotificationParameter("Currency")]
        public string Currency { get; set; }
    }
}

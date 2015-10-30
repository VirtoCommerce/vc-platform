using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notifications
{
    public class OrderSentEmailNotification : EmailNotification
    {
        public OrderSentEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }

        [NotificationParameter("Readable order number")]
        public string OrderNumber { get; set; }

        [NotificationParameter("Sent date")]
        public DateTime SentOrderDate { get; set; }

        [NotificationParameter("Number of sent shipments")]
        public int NumberOfShipments { get; set; }

        [NotificationParameter("Readable shipment numbers")]
        public string[] ShipmentsNumbers { get; set; }
    }
}

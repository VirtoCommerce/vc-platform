using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notifications
{
    public class CancelOrderEmailNotification : EmailNotification
    {
        public CancelOrderEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }
        
        [NotificationParameter("Readable order number")]
        public string OrderNumber { get; set; }

        [NotificationParameter("Cancel order reason")]
        public string CancelationReason { get; set; }
    }
}

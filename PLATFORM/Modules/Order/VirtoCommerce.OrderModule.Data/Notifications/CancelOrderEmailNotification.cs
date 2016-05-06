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
    public class CancelOrderEmailNotification : OrderEmailNotificationBase
    {
        public CancelOrderEmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway) { }

        private string _cancelReason;
        [NotificationParameter("Cancel order reason")]
        public string CancelationReason 
        {
            get
            {
                if (_cancelReason == null && CustomerOrder != null)
                {
                    _cancelReason = CustomerOrder.CancelReason;
                }
                return _cancelReason;
            }
            set
            {
                _cancelReason = value;
            }
        }
    }
}

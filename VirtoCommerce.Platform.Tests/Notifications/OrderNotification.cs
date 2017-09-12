using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Tests.Notifications.Models;

namespace VirtoCommerce.Platform.Tests.Notifications
{
    public class OrderNotification : Notification
    {
        public OrderNotification(INotificationSendingGateway notificationSendingGateway) : base(notificationSendingGateway)
        {
            NotificationTemplate = new NotificationTemplate();
            NotificationTemplate.Body = @"
{% for address in customer_order.addresses %}
 {{ address.first_name }} - {{ address.last_name }}
{% endfor %}";
        }

        [NotificationParameter("Customer Order")]
        public CustomerOrder CustomerOrder { get; set; }
    }
}
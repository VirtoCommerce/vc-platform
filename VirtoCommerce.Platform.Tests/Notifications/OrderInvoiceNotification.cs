using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Tests.Notifications.Models;

namespace VirtoCommerce.Platform.Tests.Notifications
{
    public class OrderInvoiceNotification : Notification
    {
        public OrderInvoiceNotification(INotificationSendingGateway notificationSendingGateway) : base(notificationSendingGateway)
        {
            NotificationTemplate = new NotificationTemplate();
            NotificationTemplate.Body = @"
<table style=""font-size:12px;"">
    {% for line_item in customer_order.items %}
            <tr>
                <td style=""text-align: center; vertical-align: top; width: 30px;""><p>{{forloop.index}}</p></td>
                <td style=""vertical-align: top; width: 200px;""><p>{{line_item.name}}</p><p style=""color: #999; font-size: 11px;""> SKU:{{line_item.sku}}</p></td>
                <td style=""text-align: center; vertical-align: top; width: 100px;""><p>${{line_item.placed_price | round: 2}}</p></td>
                <td style=""text-align: center; vertical-align: top; width: 100px;""><p>{{line_item.quantity}}</p></td>
                <td style=""text-align: center; vertical-align: top; width: 100px;""><p>${{line_item.extended_price | round: 2}}</p></td>
            </tr>
    {% endfor %}
</table>";
        }

        [NotificationParameter("Customer Order")]
        public CustomerOrder CustomerOrder { get; set; }
    }
}
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Tests.Notifications.Models;

namespace VirtoCommerce.Platform.Tests.Notifications
{
    public class AddressNotification : Notification
    {
        public AddressNotification(INotificationSendingGateway notificationSendingGateway) : base(notificationSendingGateway)
        {
            NotificationTemplate = new NotificationTemplate();
            NotificationTemplate.Body = @"
<table style=""font-size:12px;"">
    <tr>
        <td style=""vertical-align: top; width: 200px;""><p>{{item.name}}</p><p style=""color: #999; font-size: 11px;""> SKU:{{item.sku}}</p></td>
        <td style=""text-align: center; vertical-align: top; width: 100px;""><p>${{item.placed_price | round: 2}}</p></td>
        <td style=""text-align: center; vertical-align: top; width: 100px;""><p>{{item.quantity}}</p></td>
        <td style=""text-align: center; vertical-align: top; width: 100px;""><p>${{item.extended_price | round: 2}}</p></td>
    </tr>
</table>
<br>
{{ address.first_name }} - {{ address.last_name }}";
        }

        [NotificationParameter("Item")]
        public LineItem Item { get; set; }

        [NotificationParameter("Address")]
        public Address Address { get; set; }
    }
}
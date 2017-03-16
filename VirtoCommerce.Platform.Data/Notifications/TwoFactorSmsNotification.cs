using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class TwoFactorSmsNotification : SmsNotification
    {
        public TwoFactorSmsNotification(ISmsNotificationSendingGateway gateway)
            : base(gateway)
        {
        }

        [NotificationParameter("Two factor authentication token")]
        public string Token { get; set; }
    }
}

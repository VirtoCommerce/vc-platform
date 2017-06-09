using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class TwoFactorEmailNotification : EmailNotification
    {
        public TwoFactorEmailNotification(IEmailNotificationSendingGateway gateway)
            : base(gateway)
        {
        }

        [NotificationParameter("Two factor authentication token")]
        public string Token { get; set; }
    }
}

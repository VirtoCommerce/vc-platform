using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class ResetPasswordSmsNotification : SmsNotification
    {

        public ResetPasswordSmsNotification(ISmsNotificationSendingGateway gateway)
            : base(gateway)
        {
        }

        [NotificationParameter("Reset password token")]
        public string Token { get; set; }
    }
}

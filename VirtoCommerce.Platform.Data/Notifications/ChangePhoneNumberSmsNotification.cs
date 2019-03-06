using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class ChangePhoneNumberSmsNotification : SmsNotification
    {
        public ChangePhoneNumberSmsNotification(ISmsNotificationSendingGateway gateway)
            : base(gateway)
        {
        }

        [NotificationParameter("Change phone number token")]
        public string Token { get; set; }
    }
}

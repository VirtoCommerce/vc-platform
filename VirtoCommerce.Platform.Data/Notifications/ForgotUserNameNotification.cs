using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class ForgotUserNameNotification : EmailNotification
    {
        public ForgotUserNameNotification(IEmailNotificationSendingGateway gateway) : base(gateway)
        {
        }

        /// <summary>
        /// User login
        /// </summary>
        [NotificationParameter("User name for sing in")]
        public string UserName { get; set; }
    }
}

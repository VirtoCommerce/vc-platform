using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class RegistrationEmailNotification : EmailNotification
    {
        public RegistrationEmailNotification(IEmailNotificationSendingGateway emailNotificationSendingGateway)
            : base(emailNotificationSendingGateway)
        {
        }

        /// <summary>
        /// User login
        /// </summary>
        [NotificationParameter("User login")]
        public string Login { get; set; }

        /// <summary>
        /// User firstname
        /// </summary>
        [NotificationParameter("User firstname")]
        public string FirstName { get; set; }

        /// <summary>
        /// User lastname
        /// </summary>
        [NotificationParameter("User lastname")]
        public string LastName { get; set; }
    }
}

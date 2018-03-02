using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Notifications;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class ApplicationPhoneNumberTokenProvider : PhoneNumberTokenProvider<ApplicationUser>
    {
        private readonly INotificationManager _notificationManager;

        public ApplicationPhoneNumberTokenProvider(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public override async Task NotifyAsync(string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            var notification = _notificationManager.GetNewNotification<TwoFactorSmsNotification>();

            notification.Recipient = await manager.GetPhoneNumberAsync(user.Id);
            notification.Token = token;

            _notificationManager.SendNotification(notification);
        }
    }
}

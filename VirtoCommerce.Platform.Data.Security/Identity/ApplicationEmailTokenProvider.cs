﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Notifications;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class ApplicationEmailTokenProvider : EmailTokenProvider<ApplicationUser>
    {
        private readonly INotificationManager _notificationManager;

        public ApplicationEmailTokenProvider(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        public override async Task NotifyAsync(string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            var notification = _notificationManager.GetNewNotification<TwoFactorEmailNotification>();

            notification.Recipient = await manager.GetEmailAsync(user.Id);
            notification.Token = token;

            _notificationManager.SendNotification(notification);
        }
    }
}

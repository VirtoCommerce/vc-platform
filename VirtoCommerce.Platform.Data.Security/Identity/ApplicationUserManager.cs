using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Notifications;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly INotificationManager _notificationManager;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IDataProtectionProvider dataProtectionProvider, INotificationManager notificationManager)
            : base(store)
        {
            _notificationManager = notificationManager;

            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true, //Cannot require emails because users can be created from wpf admin and username not enforced to be as email
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            RegisterTwoFactorProvider("PhoneNumberTokenProvider", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "{0}" // Token
            });
            RegisterTwoFactorProvider("EmailTokenProvider", new EmailTokenProvider<ApplicationUser>
            {
                BodyFormat = "{0}" // Token
            });

            if (dataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        public override async Task SendEmailAsync(string userId, string subject, string body)
        {
            var notification = _notificationManager.GetNewNotification<TwoFactorEmailNotification>();

            notification.Recipient = await GetEmailAsync(userId);
            notification.Token = body;

            _notificationManager.SendNotification(notification);
        }

        public override async Task SendSmsAsync(string userId, string message)
        {
            var notification = _notificationManager.GetNewNotification<TwoFactorSmsNotification>();

            notification.Recipient = await GetPhoneNumberAsync(userId);
            notification.Token = message;

            _notificationManager.SendNotification(notification);
        }
    }
}

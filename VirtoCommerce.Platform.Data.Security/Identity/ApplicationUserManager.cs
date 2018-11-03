using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IDataProtectionProvider dataProtectionProvider, INotificationManager notificationManager, AuthenticationOptions authenticationOptions)
            : base(store)
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = authenticationOptions.AllowOnlyAlphanumericUserNames,
                RequireUniqueEmail = authenticationOptions.RequireUniqueEmail,
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = authenticationOptions.PasswordRequiredLength,
                RequireNonLetterOrDigit = authenticationOptions.PasswordRequireNonLetterOrDigit,
                RequireDigit = authenticationOptions.PasswordRequireDigit,
                RequireLowercase = authenticationOptions.PasswordRequireLowercase,
                RequireUppercase = authenticationOptions.PasswordRequireUppercase,
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = authenticationOptions.UserLockoutEnabledByDefault;
            DefaultAccountLockoutTimeSpan = authenticationOptions.DefaultAccountLockoutTimeSpan;
            MaxFailedAccessAttemptsBeforeLockout = authenticationOptions.MaxFailedAccessAttemptsBeforeLockout;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("PhoneCode", new ApplicationPhoneNumberTokenProvider(notificationManager));
            RegisterTwoFactorProvider("EmailCode", new ApplicationEmailTokenProvider(notificationManager));

            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity")) { TokenLifespan = authenticationOptions.DefaultTokenLifespan };
            }
        }
    }
}

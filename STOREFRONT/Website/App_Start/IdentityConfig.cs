namespace VirtoCommerce.Web
{
    #region
    using System;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.Web.Models.Convertors;
    using VirtoCommerce.Web.Models.Security;

    #endregion

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserStore : IUserStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser, string>,
        IUserPasswordStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser, string>,
        IUserEmailStore<ApplicationUser>
    {
        #region Public Properties
        public SecurityClient SecurityClient
        {
            get
            {
                return ClientContext.Clients.CreateSecurityClient();
            }
        }
        #endregion

        #region Public Methods and Operators
        public static ApplicationUserStore Create()
        {
            return new ApplicationUserStore();
        }

        public async Task CreateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await this.SecurityClient.CreateAsync(user.ToServiceModel());
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await this.SecurityClient.DeleteAsync(user.Id);
        }

        public void Dispose()
        {
            this.SecurityClient.Dispose();
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = await this.SecurityClient.FindByIdAsync(userId);
            return user.ToWebModel();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = await this.SecurityClient.FindByNameAsync(userName);
            return user.ToWebModel();
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return
                Task.FromResult(
                    user.LockoutEndDateUtc.HasValue
                        ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                        : new DateTimeOffset());
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await this.SecurityClient.UpdateAsync(user.ToServiceModel());
        }
        #endregion
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        #region Constructors and Destructors
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }
        #endregion

        #region Public Methods and Operators
        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(context.Get<ApplicationUserStore>());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                                    {
                                        AllowOnlyAlphanumericUserNames = false,
                                        //RequireUniqueEmail = true, //Cannot require emails because users can be created from wpf admin and username not enforced to be as email
                                    };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
                                        {
                                            RequiredLength = 5,
                                            RequireNonLetterOrDigit = false,
                                            RequireDigit = false,
                                            RequireLowercase = false,
                                            RequireUppercase = false,
                                        };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider(
                "PhoneCode",
                new PhoneNumberTokenProvider<ApplicationUser> { MessageFormat = "Your security code is: {0}" });
            manager.RegisterTwoFactorProvider(
                "EmailCode",
                new EmailTokenProvider<ApplicationUser>
                {
                    Subject = "SecurityCode",
                    BodyFormat = "Your security code is {0}"
                });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
        #endregion
    }

    public class EmailService : IIdentityMessageService
    {
        #region Public Methods and Operators
        public Task SendAsync(IdentityMessage message)
        {
            var client = new SmtpClient();
            var eMessage = new MailMessage { Body = message.Body, Subject = message.Subject, IsBodyHtml = true };
            eMessage.To.Add(message.Destination);
            return client.SendMailAsync(eMessage);
        }
        #endregion
    }

    public class SmsService : IIdentityMessageService
    {
        #region Public Methods and Operators
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
        #endregion
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        #region Constructors and Destructors
        public ApplicationSignInManager(
            ApplicationUserManager userManager,
            IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
        #endregion

        #region Public Methods and Operators
        public static ApplicationSignInManager Create(
            IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(
                context.GetUserManager<ApplicationUserManager>(),
                context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return this.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }
        #endregion
    }
}
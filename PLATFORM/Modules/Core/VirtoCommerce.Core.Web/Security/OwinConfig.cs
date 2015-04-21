using System;
using System.Linq;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.CoreModule.Web.Hangfire;
using VirtoCommerce.CoreModule.Web.Security.Hmac;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class OwinConfig
    {
        public const string PublicClientId = "web";

        public static void Configure(IAppBuilder app, IUnityContainer container, string databaseConnectionStringName)
        {
            // Configure the db context, user manager and role manager to use a single instance per request
            app.CreatePerOwinContext(() => new SecurityDbContext(databaseConnectionStringName));
            app.CreatePerOwinContext<ApplicationUserStore>(ApplicationUserStore.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //LoginPath = new PathString("/Account/Logon"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(1440),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1),
                AllowInsecureHttp = true
            });

            var settingsManager = container.Resolve<ISettingsManager>();
            var cacheRepository = container.Resolve<ICacheRepository>();
            var cacheSettings = new[]
            {
                new CacheSettings(HmacAuthenticationHandler.CacheGroup, TimeSpan.FromSeconds(settingsManager.GetValue(CachingSecurityService.CacheTimeout, 60)), "", true)
            };
            var cacheManager = new CacheManager(provider => cacheRepository, group => cacheSettings.FirstOrDefault(settings => settings.Group == group));

            app.UseHmacAuthentication(new HmacAuthenticationOptions
            {
                ApiCredentialsProvider = container.Resolve<IApiAccountProvider>(),
                IdentityProvider = container.Resolve<IClaimsIdentityProvider>(),
                CacheManager = cacheManager,
            });

            var permissionService = container.Resolve<IPermissionService>();
            app.UseHangfire(config =>
            {
                config.UseUnityActivator(container);
                config.UseSqlServerStorage(databaseConnectionStringName, new SqlServerStorageOptions { PrepareSchemaIfNecessary = false });
                config.UseAuthorizationFilters(new PermissionBasedAuthorizationFilter(permissionService) { Permission = PredefinedPermissions.BackgroundJobsManage });
                config.UseServer();
            });
        }
    }
}

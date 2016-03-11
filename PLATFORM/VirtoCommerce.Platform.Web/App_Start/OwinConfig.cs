using System;
using CacheManager.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Authentication.ApiKeys;
using VirtoCommerce.Platform.Data.Security.Authentication.Hmac;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Web
{
    public class OwinConfig
    {
        public const string PublicClientId = "web";

        public static void Configure(IAppBuilder app, IUnityContainer container, AuthenticationOptions authenticationOptions)
        {
            app.CreatePerOwinContext(() => container.Resolve<SecurityDbContext>());
            app.CreatePerOwinContext(() => container.Resolve<ApplicationUserManager>());

            if (authenticationOptions.CookiesEnabled)
            {
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
                            validateInterval: authenticationOptions.CookiesValidateInterval,
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                    }
                });
            }

            if (authenticationOptions.BearerTokensEnabled)
            {
                app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
                {
                    TokenEndpointPath = new PathString("/Token"),
                    AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                    Provider = new ApplicationOAuthProvider(PublicClientId),
                    AccessTokenExpireTimeSpan = authenticationOptions.BearerTokensExpireTimeSpan,
                    AllowInsecureHttp = true
                });
            }

            if (authenticationOptions.HmacEnabled || authenticationOptions.ApiKeysEnabled)
            {
                var apiAccountProvider = container.Resolve<IApiAccountProvider>();
                var claimsIdentityProvider = container.Resolve<IClaimsIdentityProvider>();
                var cacheManager = container.Resolve<ICacheManager<object>>();


                if (authenticationOptions.HmacEnabled)
                {
                    app.UseHmacAuthentication(new HmacAuthenticationOptions
                    {
                        ApiCredentialsProvider = apiAccountProvider,
                        IdentityProvider = claimsIdentityProvider,
                        CacheManager = cacheManager,
                        SignatureValidityPeriod = authenticationOptions.HmacSignatureValidityPeriod
                    });
                }

                if (authenticationOptions.ApiKeysEnabled)
                {
                    app.UseApiKeysAuthentication(new ApiKeysAuthenticationOptions
                    {
                        ApiCredentialsProvider = apiAccountProvider,
                        IdentityProvider = claimsIdentityProvider,
                        CacheManager = cacheManager,
                        HttpHeaderName = authenticationOptions.ApiKeysHttpHeaderName,
                        QueryStringParameterName = authenticationOptions.ApiKeysQueryStringParameterName
                    });
                }
            }

            app.Use<CurrentUserOwinMiddleware>(container.Resolve<Func<ICurrentUser>>());
        }
    }
}

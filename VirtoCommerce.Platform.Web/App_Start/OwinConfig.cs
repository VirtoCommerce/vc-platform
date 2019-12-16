using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;
using CacheManager.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Authentication.ApiKeys;
using VirtoCommerce.Platform.Data.Security.Authentication.Hmac;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Data.Security.Services;
using AuthenticationOptions = VirtoCommerce.Platform.Core.Security.AuthenticationOptions;

namespace VirtoCommerce.Platform.Web
{
    public class OwinConfig
    {
        public const string PublicClientId = "web";

        public static void Configure(IAppBuilder app, IUnityContainer container)
        {
            app.CreatePerOwinContext(() => container.Resolve<SecurityDbContext>());
            app.CreatePerOwinContext(() => container.Resolve<ApplicationUserManager>());

            var origins = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:CORS:AllowedOrigins");

            if (!string.IsNullOrEmpty(origins))
            {
                var corsPolicy = new CorsPolicy
                {
                    AllowAnyMethod = true,
                    AllowAnyHeader = true
                };

                var originsArray = origins.Split(';');
                if (originsArray.Contains("*"))
                {
                    corsPolicy.AllowAnyOrigin = true;
                }
                else
                {
                    foreach (var origin in originsArray)
                    {
                        corsPolicy.Origins.Add(origin);
                    }
                }

                var corsOptions = new CorsOptions
                {
                    PolicyProvider = new CorsPolicyProvider
                    {
                        PolicyResolver = context => Task.FromResult(corsPolicy)
                    }
                };

                app.UseCors(corsOptions);
            }


            var authenticationOptions = container.Resolve<AuthenticationOptions>();

            if (authenticationOptions.CookiesEnabled)
            {
                // Enable the application to use a cookie to store information for the signed in user
                // and to use a cookie to temporarily store information about a user logging in with a third party login provider
                // Configure the sign in cookie
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationMode = authenticationOptions.AuthenticationMode,
                    AuthenticationType = authenticationOptions.AuthenticationType,
                    CookieDomain = authenticationOptions.CookieDomain,
                    CookieHttpOnly = authenticationOptions.CookieHttpOnly,
                    CookieName = authenticationOptions.CookieName,
                    CookiePath = authenticationOptions.CookiePath,
                    CookieSecure = authenticationOptions.CookieSecure,
                    ExpireTimeSpan = authenticationOptions.ExpireTimeSpan,
                    LoginPath = authenticationOptions.LoginPath,
                    LogoutPath = authenticationOptions.LogoutPath,
                    ReturnUrlParameter = authenticationOptions.ReturnUrlParameter,
                    SlidingExpiration = authenticationOptions.SlidingExpiration,
                    Provider = new CookieAuthenticationProvider
                    {
                        // Enables the application to validate the security stamp when the user logs in.
                        // This is a security feature which is used when you change a password or add an external login to your account.  
                        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: authenticationOptions.CookiesValidateInterval,
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, authenticationOptions.AuthenticationType))
                    }
                });
            }

            if (authenticationOptions.BearerTokensEnabled)
            {
                container.RegisterType<IRefreshTokenService, RefreshTokenService>();

                var refreshTokenService = container.Resolve<IRefreshTokenService>();
                var refreshTokenProvider = new RefreshTokenProvider(authenticationOptions.RefreshTokenExpireTimeSpan, refreshTokenService);
                var eventPublisher = container.Resolve<IEventPublisher>();
                var securityService = container.Resolve<ISecurityService>();

                app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
                {
                    TokenEndpointPath = new PathString("/Token"),
                    AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                    Provider = new ApplicationOAuthProvider(PublicClientId, authenticationOptions, eventPublisher, securityService),
                    RefreshTokenProvider = refreshTokenProvider,
                    AccessTokenExpireTimeSpan = authenticationOptions.AccessTokenExpireTimeSpan,
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

            if (authenticationOptions.AzureAdAuthenticationEnabled)
            {
                // Cookie authentication to temporarily store external authentication data.
                // NOTE: AuthenticationType should not change - it is used internally by ASP.NET external authentication code!
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                    AuthenticationMode = AuthenticationMode.Passive
                });

                var authority = authenticationOptions.AzureAdInstance + authenticationOptions.AzureAdTenantId;
                app.UseOpenIdConnectAuthentication(
                    new OpenIdConnectAuthenticationOptions
                    {
                        AuthenticationType = authenticationOptions.AzureAdAuthenticationType,
                        Caption = authenticationOptions.AzureAdAuthenticationCaption,
                        ClientId = authenticationOptions.AzureAdApplicationId,
                        Authority = authority,
                        AuthenticationMode = AuthenticationMode.Passive,
                        SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie
                    });
            }

            app.Use<CurrentUserOwinMiddleware>(container.Resolve<Func<ICurrentUser>>());
        }
    }
}

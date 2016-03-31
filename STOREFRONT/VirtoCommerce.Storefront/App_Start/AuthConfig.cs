using System;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront
{
    public class AuthConfig
    {
        public static void ConfigureAuth(IAppBuilder app, Func<IStorefrontUrlBuilder> urlBuilderFactory)
        {
            // Configure the db context, user manager and role manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationUserStore.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    CookieName = StorefrontConstants.AuthenticationCookie,
                    Provider = new CookieAuthenticationProvider
                    {
                        OnApplyRedirect = context => ApplyRedirect(context, urlBuilderFactory)
                    }
                });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            if (ConfigurationManager.AppSettings.GetValue("OAuth2.Facebook.Enabled", false))
            {
                var facebookOptions = new FacebookAuthenticationOptions
                {
                    AppId = ConfigurationManager.AppSettings["OAuth2.Facebook.AppId"],
                    AppSecret = ConfigurationManager.AppSettings["OAuth2.Facebook.Secret"]
                };
                app.UseFacebookAuthentication(facebookOptions);
            }

            if (ConfigurationManager.AppSettings.GetValue("OAuth2.Google.Enabled", false))
            {
                var googleOptions = new GoogleOAuth2AuthenticationOptions
                {
                    ClientId = ConfigurationManager.AppSettings["OAuth2.Google.ClientId"],
                    ClientSecret = ConfigurationManager.AppSettings["OAuth2.Google.Secret"]
                };
                app.UseGoogleAuthentication(googleOptions);
            }
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context, Func<IStorefrontUrlBuilder> urlBuilderFactory)
        {
            Uri absoluteUri;
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out absoluteUri))
            {
                var path = PathString.FromUriComponent(absoluteUri);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                {
                    var urlBuilder = urlBuilderFactory();
                    context.RedirectUri = urlBuilder.ToAppAbsolute(context.Options.LoginPath.ToString()) + new QueryString(context.Options.ReturnUrlParameter, context.Request.Uri.AbsoluteUri);
                }
            }

            context.Response.Redirect(context.RedirectUri);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    }
}
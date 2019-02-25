using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace VirtoCommerce.Platform.Core.Security
{
    public class AuthenticationOptions
    {
        #region User validation

        public bool AllowOnlyAlphanumericUserNames { get; set; }
        public bool RequireUniqueEmail { get; set; }

        #endregion

        #region Password validation

        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonLetterOrDigit { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireUppercase { get; set; }

        #endregion

        #region Lockout

        public bool UserLockoutEnabledByDefault { get; set; }
        public TimeSpan DefaultAccountLockoutTimeSpan { get; set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        #endregion

        #region Cookies, tokens, API keys

        public TimeSpan DefaultTokenLifespan { get; set; }

        public bool CookiesEnabled { get; set; }
        public TimeSpan CookiesValidateInterval { get; set; }

        public bool BearerTokensEnabled { get; set; }
        public TimeSpan AccessTokenExpireTimeSpan { get; set; }
        public TimeSpan RefreshTokenExpireTimeSpan { get; set; }
        //The list of permissions that issued for user when using Barrier token authorization and it will be used to authorize some non-AJAX requests where is impossible to add Barrier Authorization header
        public string BearerAuthorizationLimitedCookiePermissions { get; set; }

        public bool HmacEnabled { get; set; }
        public TimeSpan HmacSignatureValidityPeriod { get; set; }

        public bool ApiKeysEnabled { get; set; }
        public string ApiKeysHttpHeaderName { get; set; }
        public string ApiKeysQueryStringParameterName { get; set; }

        #endregion

        #region Cookie

        /// <summary>
        /// The AuthenticationType in the options corresponds to the IIdentity AuthenticationType property. A different
        /// value may be assigned in order to use the same authentication middleware type more than once in a pipeline.
        /// </summary>
        public string AuthenticationType { get; set; }

        /// <summary>
        /// If Active the authentication middleware alter the request user coming in and
        /// alter 401 Unauthorized responses going out. If Passive the authentication middleware will only provide
        /// identity and alter responses when explicitly indicated by the AuthenticationType.
        /// </summary>
        public AuthenticationMode AuthenticationMode { get; set; }

        private string _cookieName;

        /// <summary>
        /// Determines the cookie name used to persist the identity. The default value is ".AspNet.Cookies".
        /// This value should be changed if you change the name of the AuthenticationType, especially if your
        /// system uses the cookie authentication middleware multiple times.
        /// </summary>
        public string CookieName
        {
            get { return _cookieName; }
            set
            {
                _cookieName = value ?? throw new ArgumentNullException("value");
            }
        }

        /// <summary>
        /// Determines the domain used to create the cookie. Is not provided by default.
        /// </summary>
        public string CookieDomain { get; set; }

        /// <summary>
        /// Determines the path used to create the cookie. The default value is "/" for highest browser compatability.
        /// </summary>
        public string CookiePath { get; set; }

        /// <summary>
        /// Determines if the browser should allow the cookie to be accessed by client-side javascript. The
        /// default is true, which means the cookie will only be passed to http requests and is not made available
        /// to script on the page.
        /// </summary>
        public bool CookieHttpOnly { get; set; }

        /// <summary>
        /// Determines if the cookie should only be transmitted on HTTPS request. The default is to limit the cookie
        /// to HTTPS requests if the page which is doing the SignIn is also HTTPS. If you have an HTTPS sign in page
        /// and portions of your site are HTTP you may need to change this value.
        /// </summary>
        public CookieSecureOption CookieSecure { get; set; }

        /// <summary>
        /// Controls how much time the cookie will remain valid from the point it is created. The expiration
        /// information is in the protected cookie ticket. Because of that an expired cookie will be ignored 
        /// even if it is passed to the server after the browser should have purged it 
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; set; }

        /// <summary>
        /// The SlidingExpiration is set to true to instruct the middleware to re-issue a new cookie with a new
        /// expiration time any time it processes a request which is more than halfway through the expiration window.
        /// </summary>
        public bool SlidingExpiration { get; set; }

        /// <summary>
        /// The LoginPath property informs the middleware that it should change an outgoing 401 Unauthorized status
        /// code into a 302 redirection onto the given login path. The current url which generated the 401 is added
        /// to the LoginPath as a query string parameter named by the ReturnUrlParameter. Once a request to the
        /// LoginPath grants a new SignIn identity, the ReturnUrlParameter value is used to redirect the browser back  
        /// to the url which caused the original unauthorized status code.
        /// 
        /// If the LoginPath is null or empty, the middleware will not look for 401 Unauthorized status codes, and it will
        /// not redirect automatically when a login occurs.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "By design")]
        public PathString LoginPath { get; set; }

        /// <summary>
        /// If the LogoutPath is provided the middleware then a request to that path will redirect based on the ReturnUrlParameter.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Logout", Justification = "By design")]
        public PathString LogoutPath { get; set; }

        /// <summary>
        /// The ReturnUrlParameter determines the name of the query string parameter which is appended by the middleware
        /// when a 401 Unauthorized status code is changed to a 302 redirect onto the login path. This is also the query 
        /// string parameter looked for when a request arrives on the login path or logout path, in order to return to the 
        /// original url after the action is performed.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "ReturnUrl is the name of a querystring parameter")]
        public string ReturnUrlParameter { get; set; }

        #endregion

        #region AzureAD authentication

        /// <summary>
        /// Determines whether the user authentication via Azure Active Directory is enabled.
        /// </summary>
        public bool AzureAdAuthenticationEnabled { get; set; }

        /// <summary>
        /// Sets AuthenticationType value for Azure AD authentication provider.
        /// </summary>
        public string AzureAdAuthenticationType { get; set; }

        /// <summary>
        /// Sets human-readable caption for Azure AD authentication provider. It is visible on sign-in page.
        /// </summary>
        public string AzureAdAuthenticationCaption { get; set; }

        /// <summary>
        /// Application ID of the VirtoCommerce platform application registered in Azure Active Directory. It can be found 
        /// in the Azure control panel: Azure Active Directory > App registrations > (platform app) > Application ID
        /// (e.g. 01234567-89ab-cdef-0123-456789abcdef).
        /// </summary>
        public string AzureAdApplicationId { get; set; }

        /// <summary>
        /// ID of the Azure AD domain that will be used for authentication. It can be found in the Azure control panel:
        /// Azure Active Directory > Properties > Directory ID (e.g. abcdef01-2345-6789-abcd-ef0123456789).
        /// </summary>
        public string AzureAdTenantId { get; set; }

        /// <summary>
        /// URL of the Azure AD endpoint used for authentication (usually https://login.microsoftonline.com/).
        /// </summary>
        public string AzureAdInstance { get; set; }

        /// <summary>
        /// Default user type for users created by Azure AD accounts.
        /// </summary>
        public string AzureAdDefaultUserType { get; set; }

        #endregion
    }
}

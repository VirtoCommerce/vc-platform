using Microsoft.Web.WebPages.OAuth;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class AuthConfig.
    /// </summary>
    public static class AuthConfig
    {

        /// <summary>
        /// To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
        /// you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166
        /// </summary>
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterGoogleClient();
            OAuthWebSecurity.RegisterYahooClient();
        }
    }
}
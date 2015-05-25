using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security.Authentication.ApiKeys
{
    public class ApiKeysAuthenticationHandler : ApiAuthenticationHandler<ApiKeysAuthenticationOptions>
    {
        protected override string ExtractUserIdFromRequest()
        {
            string userId = null;

            var appId = ExtractAppIdFromRequest();
            if (!string.IsNullOrEmpty(appId))
            {
                var apiAccount = Options.ApiCredentialsProvider.GetAccountByAppId(ApiAccountType.Simple, appId);
                if (apiAccount != null)
                {
                    userId = apiAccount.AccountId;
                }
            }

            return userId;
        }


        private string ExtractAppIdFromRequest()
        {
            // Check the Authorization header
            var appId = GetAuthenticationHeaderCredentials();

            if (string.IsNullOrWhiteSpace(appId))
            {
                // Check custom header
                appId = Request.Headers.Get(Options.HttpHeaderName);

                if (string.IsNullOrWhiteSpace(appId))
                {
                    // Check query string
                    appId = Request.Query[Options.QueryStringParameterName];
                }
            }

            if (appId != null)
            {
                appId = appId.Trim();
            }

            return appId;
        }
    }
}

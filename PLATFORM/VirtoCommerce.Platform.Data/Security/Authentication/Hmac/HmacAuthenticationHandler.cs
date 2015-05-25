using System;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Hmac
{
    public class HmacAuthenticationHandler : ApiAuthenticationHandler<HmacAuthenticationOptions>
    {
        protected override string ExtractUserIdFromRequest()
        {
            string userId = null;

            var credentials = GetAuthenticationHeaderCredentials();

            ApiRequestSignature signature;
            if (ApiRequestSignature.TryParse(credentials, out signature))
            {
                if ((DateTime.UtcNow - signature.Timestamp).Duration() < Options.SignatureValidityPeriod)
                {
                    var apiAccount = Options.ApiCredentialsProvider.GetAccountByAppId(ApiAccountType.Hmac, signature.AppId);
                    if (apiAccount != null && IsValidSignature(signature, apiAccount))
                    {
                        userId = apiAccount.AccountId;
                    }
                }
            }

            return userId;
        }


        private bool IsValidSignature(ApiRequestSignature signature, ApiAccountEntity credentials)
        {
            var parameters = new[]
            {
                new NameValuePair(null, signature.AppId),
                new NameValuePair(null, signature.TimestampString)
            };

            var validSignature = HmacUtility.GetHashString(Options.HmacFactory, credentials.SecretKey, parameters);
            var isValid = string.Equals(signature.Hash, validSignature, StringComparison.OrdinalIgnoreCase);
            return isValid;
        }
    }
}

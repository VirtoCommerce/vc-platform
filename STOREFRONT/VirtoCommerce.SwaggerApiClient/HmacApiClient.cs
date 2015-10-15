using System.Collections.Generic;
using System.Security.Cryptography;
using VirtoCommerce.SwaggerApiClient.Client;

namespace VirtoCommerce.SwaggerApiClient
{
    public class HmacApiClient : ApiClient
    {
        private readonly string _appId;
        private readonly string _secretKey;

        public HmacApiClient(string basePath, string appId, string secretKey)
            : base(basePath)
        {
            _appId = appId;
            _secretKey = secretKey;
        }

        public override void UpdateParamsForAuth(Dictionary<string, string> queryParams, Dictionary<string, string> headerParams, string[] authSettings)
        {
            base.UpdateParamsForAuth(queryParams, headerParams, authSettings);

            var signature = new ApiRequestSignature { AppId = _appId };

            var parameters = new[]
            {
                new NameValuePair(null, _appId),
                new NameValuePair(null, signature.TimestampString)
            };

            signature.Hash = HmacUtility.GetHashString(key => new HMACSHA256(key), _secretKey, parameters);
            headerParams["Authorization"] = "HMACSHA256 " + signature;
        }
    }
}

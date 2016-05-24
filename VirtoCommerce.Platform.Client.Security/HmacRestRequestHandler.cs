using System.Security.Cryptography;
using RestSharp;

namespace VirtoCommerce.Platform.Client.Security
{
    public class HmacRestRequestHandler
    {
        private readonly string _appId;
        private readonly string _secretKey;

        public HmacRestRequestHandler(string appId, string secretKey)
        {
            _appId = appId;
            _secretKey = secretKey;
        }

        public void PrepareRequest(IRestRequest request)
        {
            var signature = new ApiRequestSignature { AppId = _appId };

            var parameters = new[]
            {
                new NameValuePair(null, _appId),
                new NameValuePair(null, signature.TimestampString)
            };

            signature.Hash = HmacUtility.GetHashString(key => new HMACSHA256(key), _secretKey, parameters);
            request.AddHeader("Authorization", "HMACSHA256 " + signature);
        }
    }
}

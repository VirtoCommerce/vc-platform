using System.Collections.Generic;
using System.Security.Cryptography;
using RestSharp;
using VirtoCommerce.Client.Client;

namespace VirtoCommerce.Client
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

        protected override RestRequest PrepareRequest(string path, Method method, Dictionary<string, string> queryParams, object postBody, Dictionary<string, string> headerParams,
            Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
        {
            var request = base.PrepareRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, pathParams, contentType);

            var signature = new ApiRequestSignature { AppId = _appId };

            var parameters = new[]
            {
                new NameValuePair(null, _appId),
                new NameValuePair(null, signature.TimestampString)
            };

            signature.Hash = HmacUtility.GetHashString(key => new HMACSHA256(key), _secretKey, parameters);
            request.AddHeader("Authorization", "HMACSHA256 " + signature);

            return request;
        }
    }
}

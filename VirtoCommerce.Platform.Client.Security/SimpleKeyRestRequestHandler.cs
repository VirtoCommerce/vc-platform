using RestSharp;

namespace VirtoCommerce.Platform.Client.Security
{
    public class SimpleKeyRestRequestHandler
    {
        private readonly string _simpleApiKey;

        public SimpleKeyRestRequestHandler(string simpleApiKey)
        {
            _simpleApiKey = simpleApiKey;
        }

        public void PrepareRequest(IRestRequest request)
        {
            request.AddHeader("Authorization", "APIKey " + _simpleApiKey);
        }
    }
}

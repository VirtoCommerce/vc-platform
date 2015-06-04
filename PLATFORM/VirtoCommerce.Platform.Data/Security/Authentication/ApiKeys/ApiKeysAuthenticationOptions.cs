namespace VirtoCommerce.Platform.Data.Security.Authentication.ApiKeys
{
    public class ApiKeysAuthenticationOptions : ApiAuthenticationOptions
    {
        public string HttpHeaderName { get; set; }
        public string QueryStringParameterName { get; set; }

        public ApiKeysAuthenticationOptions()
            : base("APIKey")
        {
            HttpHeaderName = "api_key";
            QueryStringParameterName = "api_key";
        }
    }
}

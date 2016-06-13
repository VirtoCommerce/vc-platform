using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;

namespace VirtoCommerce.Platform.Data.Security.Authentication.ApiKeys
{
    public class ApiKeysAuthenticationMiddleware : AuthenticationMiddleware<ApiKeysAuthenticationOptions>
    {
        public ApiKeysAuthenticationMiddleware(OwinMiddleware next, ApiKeysAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<ApiKeysAuthenticationOptions> CreateHandler()
        {
            return new ApiKeysAuthenticationHandler();
        }
    }
}

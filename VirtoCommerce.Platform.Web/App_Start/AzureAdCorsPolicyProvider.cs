using System.Threading.Tasks;
using System.Web.Cors;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web
{
    public class AzureAdCorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly CorsPolicy _policy;
        private readonly AuthenticationOptions _authenticationOptions;

        public AzureAdCorsPolicyProvider(AuthenticationOptions authenticationOptions)
        {
            _authenticationOptions = authenticationOptions;

            _policy = new CorsPolicy()
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            _policy.Origins.Add(authenticationOptions.AzureAdInstance);
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(IOwinRequest request)
        {
            return Task.FromResult(_policy);
        }
    }
}

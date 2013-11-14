using System.Net;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Security.Services;

namespace VirtoCommerce.ManagementClient.Security.Tokens
{
	public class SecurityTokenInjector : ISecurityTokenInjector
	{

        private readonly IAuthenticationContext _authenticationContext;

        public SecurityTokenInjector(IAuthenticationContext authenticationContext)
        {
            _authenticationContext = authenticationContext;
        }

        public void InjectToken(WebHeaderCollection headers)
        {
            if (headers != null && _authenticationContext != null && _authenticationContext.IsUserAuthenticated)
            {
                _authenticationContext.UpdateToken();
                headers[HttpRequestHeader.Authorization] = string.Concat("WRAP access_token=\"", _authenticationContext.Token, "\"");
            }
            
        }
	}
}

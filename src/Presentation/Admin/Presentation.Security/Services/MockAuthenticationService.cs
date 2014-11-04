using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Security.Swt;

namespace VirtoCommerce.ManagementClient.Security.Services
{
	public class MockAuthenticationService : IAuthenticationService
	{
        private Uri _tokenIssuer = new Uri("http://mock/sts");
        private const string _tokenSignatureKey = "WRwJkQ9PgbhnIUgKuuovw/6yVAo/Dh0qrb7rqQWnsBk=";
        private List<string> _validUsers = new List<string> { "Admini", "User1" };
 

		#region IAuthenticationService

        public Task<string> AuthenticateUserAsync(string userName, string password, Uri scope)
		{
            string token = null;
            string role = userName == "Administrator" ? "adminis" : "everyone";
            if (_validUsers.Any(u => string.Equals(u, userName, StringComparison.OrdinalIgnoreCase)))
                token = SimpleWebToken.Create(_tokenIssuer, scope, DateTime.UtcNow.AddHours(1), _tokenSignatureKey, 
                    new Claim(ClaimTypes.Name, userName), 
                    new Claim(ClaimTypes.NameIdentifier, "ID"),
                    new Claim(ClaimTypes.Role, role)
                );

            return Task.FromResult(token);
            
		}

		#endregion
	}
}

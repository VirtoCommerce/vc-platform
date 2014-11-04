using System;
using System.ServiceModel;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Factories;

namespace VirtoCommerce.Foundation.Security.Services
{
    [ServiceContract(Namespace = "http://schemas.virtocommerce.com/1.0/authentication/")]
	public interface IAuthenticationService
	{
		[OperationContract]
        Task<string> AuthenticateUserAsync(string userName, string password, Uri scope);
    }
}

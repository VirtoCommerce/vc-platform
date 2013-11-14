using System;
using System.ServiceModel;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Factories;

namespace VirtoCommerce.Foundation.Security.Services
{
    [ServiceContract(Namespace = "http://schemas.virtocommerce.com/1.0/authentication/")]
	public interface IAuthenticationService
	{
		[OperationContract]
        string AuthenticateUser(string userName, string password, Uri scope);
    }
}

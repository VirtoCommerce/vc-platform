using System.Net;

namespace VirtoCommerce.Foundation.Security.Services
{
	public interface ISecurityTokenInjector
	{
		void InjectToken(WebHeaderCollection headers);
	}
}

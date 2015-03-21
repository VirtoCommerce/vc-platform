using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace VirtoCommerce.CoreModule.Web.Security
{
	public interface IClaimsIdentityProvider
	{
		Task<ClaimsIdentity> GetIdentityByUserIdAsync(IOwinContext context, string authenticationType, string userId);
	}
}

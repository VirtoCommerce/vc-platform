using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace VirtoCommerce.CoreModule.Web.Security
{
	public class ApplicationClaimsIdentityProvider : IClaimsIdentityProvider
	{
		#region IClaimsIdentityProvider Members

		public async Task<ClaimsIdentity> GetIdentityByUserIdAsync(IOwinContext context, string authenticationType, string userId)
		{
			ClaimsIdentity identity = null;

			var userManager = context.GetUserManager<ApplicationUserManager>();
			if (userManager != null)
			{
				var user = await userManager.FindByIdAsync(userId);
				if (user != null)
				{
					identity = await userManager.CreateIdentityAsync(user, authenticationType);
				}
			}

			return identity;
		}

		#endregion
	}
}

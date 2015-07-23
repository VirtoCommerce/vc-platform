using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Basic.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            UserManager<ApplicationUser> userManager = CreateUserManager();

            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, UserManager doesn't support CancellationTokens.

            ApplicationUser user = await userManager.FindAsync(userName, password);

            if (user == null)
            {
                // No user with userName/password exists.
                return null;
            }

            // Create a ClaimsIdentity with all the claims for this user.
            cancellationToken.ThrowIfCancellationRequested(); // Unfortunately, IClaimsIdenityFactory doesn't support CancellationTokens.
            ClaimsIdentity identity = await userManager.ClaimsIdentityFactory.CreateAsync(userManager, user, "Basic");
            return new ClaimsPrincipal(identity);
        }

        private static UserManager<ApplicationUser> CreateUserManager()
        {
            return new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new SecurityDbContext()));
        }
    }
}

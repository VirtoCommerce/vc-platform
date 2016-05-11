using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Authentication.Basic
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            IPrincipal result = null;

            cancellationToken.ThrowIfCancellationRequested();

            var userManager = context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ApplicationUserManager)) as ApplicationUserManager;
            var user = await userManager.FindAsync(userName, password);

            if (user != null)
            {
                var identity = await userManager.CreateIdentityAsync(user, AuthenticationType);
                result = new ClaimsPrincipal(identity);
            }

            return result;
        }
    }
}

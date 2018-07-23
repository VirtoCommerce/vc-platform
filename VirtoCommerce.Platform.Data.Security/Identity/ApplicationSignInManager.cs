using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, Core.Security.AuthenticationOptions authenticationOptions)
            : base(userManager, authenticationManager)
        {
            AuthenticationOptions = authenticationOptions;
        }

        protected Core.Security.AuthenticationOptions AuthenticationOptions { get; private set; }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, AuthenticationOptions);
        }
    }
}

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        private readonly Core.Security.AuthenticationOptions _authenticationOptions;

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, Core.Security.AuthenticationOptions authenticationOptions)
            : base(userManager, authenticationManager)
        {
            _authenticationOptions = authenticationOptions;
        }
        
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, _authenticationOptions.AuthenticationType);
        }
    }
}

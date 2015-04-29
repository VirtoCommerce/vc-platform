using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(SecurityDbContext context)
            : base(context)
        {

        }
        public static ApplicationUserStore Create(IdentityFactoryOptions<ApplicationUserStore> options, IOwinContext context)
        {
            return new ApplicationUserStore(context.Get<SecurityDbContext>());
        }
    }
}

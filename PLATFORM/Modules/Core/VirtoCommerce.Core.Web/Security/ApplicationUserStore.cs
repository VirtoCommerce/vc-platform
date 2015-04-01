using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using VirtoCommerce.Foundation.Data.Security.Identity;

namespace VirtoCommerce.CoreModule.Web.Security
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

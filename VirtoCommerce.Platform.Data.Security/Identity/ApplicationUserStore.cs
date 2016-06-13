using Microsoft.AspNet.Identity.EntityFramework;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(SecurityDbContext context)
            : base(context)
        {

        }
    }
}

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext()
            : this("VirtoCommerce")
        {
        }

        public SecurityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, false)
        {
            Database.SetInitializer<SecurityDbContext>(null);
        }

        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }
    }
}

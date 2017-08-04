using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext()
            : this(ConnectionStringHelper.GetConnectionString("VirtoCommerce"))
        {
        }

        public SecurityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, false)
        {
            Database.SetInitializer<SecurityDbContext>(null);
        }
    }
}

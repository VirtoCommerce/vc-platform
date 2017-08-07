using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Security.Identity
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext()
            : this(ConfigurationHelper.GetConnectionStringValue("VirtoCommerce"))
        {
        }

        public SecurityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, false)
        {
            Database.SetInitializer<SecurityDbContext>(null);
        }
    }
}

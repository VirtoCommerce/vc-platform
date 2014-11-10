using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VirtoCommerce.Foundation.Security;

namespace VirtoCommerce.Foundation.Data.Security.Identity
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext()
            : this(SecurityConfiguration.Instance.Connection.SqlConnectionStringName)
        {
        }

        public SecurityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, false)
        {
        }

        static SecurityDbContext()
        {
            // Set the database intializer which is run once during application start
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SecurityDbContext>());
        }

        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }
    }
}

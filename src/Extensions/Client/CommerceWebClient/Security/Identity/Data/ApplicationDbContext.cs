using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Web.Client.Security.Identity.Configs;
using VirtoCommerce.Web.Client.Security.Identity.Model;

namespace VirtoCommerce.Web.Client.Security.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : this(SecurityConfiguration.Instance.Connection.SqlConnectionStringName)
        {
        }

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString, false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}

using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Customers.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.Customers.EFCustomerRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Customers\Migrations";
            ContextKey = "VCF.Customers";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Customers.EFCustomerRepository context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

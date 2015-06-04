using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Stores.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<EFStoreRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Stores\Migrations";
            ContextKey = "VCF.Stores";
        }

        protected override void Seed(EFStoreRepository context)
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

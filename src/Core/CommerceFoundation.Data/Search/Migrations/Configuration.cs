using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Search.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<VirtoCommerce.Foundation.Data.Search.EFSearchRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Search\Migrations";
            ContextKey = "VCF.Search";
        }

        protected override void Seed(VirtoCommerce.Foundation.Data.Search.EFSearchRepository context)
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

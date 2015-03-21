using System.Data.Entity.Migrations;

namespace VirtoCommerce.Foundation.Data.Security.Identity.Migrations
{
	public sealed class Configuration : DbMigrationsConfiguration<SecurityDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			MigrationsDirectory = @"Security\Identity\Migrations";
			ContextKey = "VirtoCommerce.Foundation.Data.Security.Identity.SecurityDbContext";
		}

		protected override void Seed(SecurityDbContext context)
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

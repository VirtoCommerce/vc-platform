namespace VirtoCommerce.Foundation.Data.Customers.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using VirtoCommerce.Foundation.Data.Customers;
	using VirtoCommerce.Foundation.Data.Infrastructure;

	public sealed class Configuration : DbMigrationsConfigurationBase<EFCustomerRepository>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			MigrationsDirectory = @"Customers\Migrations";
			ContextKey = "VCF.Customers";
		}

		protected override void Seed(EFCustomerRepository context)
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

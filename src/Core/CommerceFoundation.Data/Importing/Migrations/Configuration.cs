namespace VirtoCommerce.Foundation.Data.Importing.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using VirtoCommerce.Foundation.Data.Infrastructure;
	using VirtoCommerce.Foundation.Importing.Model;

	public sealed class Configuration : DbMigrationsConfigurationBase<VirtoCommerce.Foundation.Data.Importing.EFImportingRepository>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			MigrationsDirectory = @"Importing\Migrations";
			ContextKey = "VCF.Importing";
		}

		protected override void Seed(VirtoCommerce.Foundation.Data.Importing.EFImportingRepository context)
		{
		}
	}
}
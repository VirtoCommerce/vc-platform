namespace VirtoCommerce.Foundation.Data.AppConfig.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using VirtoCommerce.Foundation.Data.Infrastructure;
	using VirtoCommerce.Foundation.AppConfig.Model;

	public sealed class Configuration : DbMigrationsConfigurationBase<EFAppConfigRepository>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			MigrationsDirectory = @"AppConfig\Migrations";
			ContextKey = "VCF.AppConfig";
		}

		protected override void Seed(EFAppConfigRepository context)
		{
		}
	}
}
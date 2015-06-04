using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;
namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public class SqlCatalogSampleDatabaseInitializer : SetupDatabaseInitializer<CatalogRepositoryImpl, VirtoCommerce.CatalogModule.Data.Migrations.Configuration>
	{
		readonly string[] _files =
		{
			"CatalogBase.sql",
			"Catalog.sql",
			"CatalogLanguage.sql",
			"VirtualCatalog.sql",
			"Property.sql",
			"PropertyAttribute.sql",
			"PropertySet.sql",
			"PropertySetProperty.sql",
			"CategoryBase.sql", 
			"Category.sql",
			"LinkedCategory.sql",
			"Item.sql",
			"ItemPropertyValue.sql",
			"CategoryItemRelation.sql",
			"EditorialReview.sql",
			"ItemAsset.sql",
			"AssociationGroup.sql",
			"Association.sql",
			"ItemRelation.sql"
		};


		protected virtual string[] GetSampleFiles()
		{
			return _files;
		}

		protected override void Seed(CatalogRepositoryImpl context)
		{
			PopulateCatalog(context);
		}

		private void PopulateCatalog(CatalogRepositoryImpl context)
		{
			foreach (var file in GetSampleFiles())
			{
				ExecuteSqlScriptFile(context, file, "Catalogs");
			}
		}
	}
}

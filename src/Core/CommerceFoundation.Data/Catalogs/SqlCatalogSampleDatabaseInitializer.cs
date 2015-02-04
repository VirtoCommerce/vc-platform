namespace VirtoCommerce.Foundation.Data.Catalogs
{
	public class SqlCatalogSampleDatabaseInitializer : SqlCatalogDatabaseInitializer
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
			"TaxCategory.sql",
			"Item.sql",
			"ItemPropertyValue.sql",
			"CategoryItemRelation.sql",
			"EditorialReview.sql",
			"ItemAsset.sql",
			"PricelistAssignment.sql",
			"Price.sql",
			"AssociationGroup.sql",
			"Association.sql",
			"ItemRelation.sql"
		};


		protected virtual string[] GetSampleFiles()
		{
			return _files;
		}

		protected override void Seed(EFCatalogRepository context)
		{
			PopulateCatalog(context);
		}

		private void PopulateCatalog(EFCatalogRepository context)
		{
			foreach (var file in GetSampleFiles())
			{
				ExecuteSqlScriptFile(context, file, "Catalogs");
			}
		}
	}
}

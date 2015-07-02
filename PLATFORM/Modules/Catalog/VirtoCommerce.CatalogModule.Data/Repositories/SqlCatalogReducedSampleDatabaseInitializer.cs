namespace VirtoCommerce.CatalogModule.Data.Repositories
{
	public class SqlCatalogReducedSampleDatabaseInitializer : SqlCatalogSampleDatabaseInitializer
	{
		readonly string[] _files =
		{
			"CatalogBase.sql",
			"Catalog.sql",
			"CatalogLanguage.sql",
			"VirtualCatalog.sql",
			"Property.reduced.sql",
			"PropertySet.sql",
			"PropertySetProperty.reduced.sql",
			"Category.sql",
			"LinkedCategory.sql",
			"Item.reduced.sql",
			"ItemPropertyValue.reduced.sql",
		
			"EditorialReview.reduced.sql",
			"ItemAsset.reduced.sql",
			"AssociationGroup.sql",
			"Association.sql",
			"ItemRelation.sql"
		};


		protected override string[] GetSampleFiles()
		{
			return _files;
		}
	}
}

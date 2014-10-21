namespace VirtoCommerce.PowerShell.Catalogs
{
    public class SqlCatalogReducedSampleDatabaseInitializer : SqlCatalogSampleDatabaseInitializer
    {
        public SqlCatalogReducedSampleDatabaseInitializer(string location)
            : base(location)
        {
        }

        public SqlCatalogReducedSampleDatabaseInitializer(string location, string connectionString)
            : base(location, connectionString)
        {
        }

        readonly string[] _files =
        { 
            "CatalogBase.sql",
            "Catalog.sql",
            "CatalogLanguage.sql",
            "VirtualCatalog.sql",
            "Property.reduced.sql",
            //"PropertyAttribute.sql",
            "PropertySet.sql",
            "PropertySetProperty.reduced.sql",
            "CategoryBase.sql", 
            "Category.sql",
            "LinkedCategory.sql",
            "TaxCategory.sql",
            "Item.reduced.sql",
            "ItemPropertyValue.reduced.sql",
            "CategoryItemRelation.reduced.sql",
            "EditorialReview.reduced.sql",
            "ItemAsset.reduced.sql",
            "PricelistAssignment.sql",
            "Price.reduced.sql",
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
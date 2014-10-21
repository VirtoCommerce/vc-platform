using System;
using System.IO;
using VirtoCommerce.PowerShell.DatabaseSetup;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Catalogs.Migrations;

namespace VirtoCommerce.PowerShell.Catalogs
{
    public class SqlCatalogSampleDatabaseInitializer : SetupDatabaseInitializer<EFCatalogRepository, Configuration>
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

	    readonly string _location = String.Empty;

        public SqlCatalogSampleDatabaseInitializer(string location)
        {
            _location = location;
        }

        public SqlCatalogSampleDatabaseInitializer(string location, string connectionString)
            : base(connectionString)
        {
            _location = location;
        }

		protected override void Seed(EFCatalogRepository context)
		{
			PopulateCatalog(context);
		}

		private void PopulateCatalog(EFCatalogRepository context)
		{
			foreach (var file in GetSampleFiles())
			{
                RunCommand(context, file, "Catalogs");
			}
		}

        protected override string ReadSql(string fileName, string modelName)
        {
            string path = String.Format("{0}/Catalogs/Data/{1}", _location, fileName);

            if (File.Exists(path))
                return File.ReadAllText(path);

            path = String.Format("{0}/Catalogs/Data/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(path))
                return File.ReadAllText(path);

            path = String.Format("{0}/bin/Catalogs/Data/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(path))
                return File.ReadAllText(path);

            path = String.Format("{0}/App_Data/Catalogs/Data/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(path))
                return File.ReadAllText(path);


            path = String.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(path))
                return File.ReadAllText(path);

            return String.Empty;
        }
	}
}
using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Catalogs.Model;


namespace VirtoCommerce.Foundation.Catalogs.Factories
{
	public class CatalogEntityFactory : FactoryBase, ICatalogEntityFactory
	{
		public CatalogEntityFactory()
		{
			RegisterStorageType(typeof(ItemAsset), "Asset");
			RegisterStorageType(typeof(Association), "Association");
			RegisterStorageType(typeof(AssociationGroup), "AssociationGroup");
            RegisterStorageType(typeof(CatalogBase), "CatalogBase");
			RegisterStorageType(typeof(Catalog), "Catalog");
            RegisterStorageType(typeof(VirtualCatalog), "VirtualCatalog");
            RegisterStorageType(typeof(Category), "Category");
			RegisterStorageType(typeof(Item), "Item");
			RegisterStorageType(typeof(Product), "Product");
			RegisterStorageType(typeof(Bundle), "Bundle");
			RegisterStorageType(typeof(Sku), "Sku");
			RegisterStorageType(typeof(Package), "Package");
			RegisterStorageType(typeof(DynamicKit), "DynamicKit");			
			RegisterStorageType(typeof(LinkedCategory), "LinkedCategory");
			RegisterStorageType(typeof(CatalogLanguage), "CatalogLanguage");
			RegisterStorageType(typeof(CategoryItemRelation), "CategoryItemRelation");
			RegisterStorageType(typeof(ItemRelation), "ItemRelation");
			RegisterStorageType(typeof(EditorialReview), "EditorialReview");

			// Pricelist
			RegisterStorageType(typeof(PricelistAssignment), "PricelistAssignment");
			RegisterStorageType(typeof(Price), "Price");
			RegisterStorageType(typeof(Pricelist), "Pricelist");

			// Properties
			RegisterStorageType(typeof(Property), "Property");
			RegisterStorageType(typeof(PropertyAttribute), "PropertyAttribute");
			RegisterStorageType(typeof(PropertySet), "PropertySet");
			RegisterStorageType(typeof(PropertyValue), "PropertyValue");
			RegisterStorageType(typeof(PropertySetProperty), "PropertySetProperty");
			RegisterStorageType(typeof(ItemPropertyValue), "ItemPropertyValue");
			RegisterStorageType(typeof(CategoryPropertyValue), "CategoryPropertyValue");

			// Packaging
			RegisterStorageType(typeof(Packaging), "Packaging");

			// Taxes
			RegisterStorageType(typeof(TaxCategory), "TaxCategory");
		}
	}
}

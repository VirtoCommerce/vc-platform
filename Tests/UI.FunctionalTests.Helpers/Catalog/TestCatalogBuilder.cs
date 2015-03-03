using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace UI.FunctionalTests.Helpers.Catalog
{
	internal class TestCatalogBuilder
	{
		private CatalogBase _catalogBase;
		private List<Item> _items = new List<Item>();

		public TestCatalogBuilder(CatalogBase catalog)
		{
			_catalogBase = catalog;
		}

		public static TestCatalogBuilder BuildCatalog(string name)
		{
			return new TestCatalogBuilder(CreateCatalog(name));
		}


		private static CatalogBase CreateCatalog(string catalogName)
		{
			var ctlg = new VirtoCommerce.Foundation.Catalogs.Model.Catalog();
			ctlg.CatalogId = catalogName;
			ctlg.Name = catalogName;
			//ctlg.StartDate = DateTime.Now.AddYears(-1);
			//ctlg.EndDate = DateTime.Now.AddYears(1);
			ctlg.WeightMeasure = WeightUnitOfMeasure.Pounds.GetHashCode();
			//ctlg.IsActive = true;
			//ctlg.SortOrder = 1;
			ctlg.DefaultLanguage = "en-us";
			ctlg.CatalogLanguages.Add(new CatalogLanguage {CatalogId = ctlg.CatalogId, Language = "en-us"});
			ctlg.CatalogLanguages.Add(new CatalogLanguage {CatalogId = ctlg.CatalogId, Language = "de-de"});

			var property1 = new Property() {Name = "Description"};
			var property2 = new Property() {Name = "Design"};
			var property3 = new Property() {Name = "Model"};

			var propertySet = new PropertySet() {CatalogId = ctlg.CatalogId, Name = "some property set", TargetType = "type"};

			propertySet.PropertySetProperties.Add(new PropertySetProperty()
				{
					PropertyId = property1.PropertyId,
					Property = property1,
					PropertySetId = propertySet.PropertySetId,
					PropertySet = propertySet
				});
			propertySet.PropertySetProperties.Add(new PropertySetProperty()
				{
					PropertyId = property2.PropertyId,
					Property = property2,
					PropertySetId = propertySet.PropertySetId,
					PropertySet = propertySet
				});
			propertySet.PropertySetProperties.Add(new PropertySetProperty()
				{
					PropertyId = property3.PropertyId,
					Property = property3,
					PropertySetId = propertySet.PropertySetId,
					PropertySet = propertySet
				});

			ctlg.PropertySets.Add(propertySet);

			return ctlg;
		}

		private static CatalogBase CreateVirtualCatalog(string catalogName)
		{
			var ctlg = new VirtualCatalog {CatalogId = catalogName, Name = catalogName};
			return ctlg;
		}

		public TestCatalogBuilder WithCategory(string categoryName)
		{
			var category = CreateCategory(categoryName, null);
			category.CatalogId = _catalogBase.CatalogId;
			category.Code = "code";
			_catalogBase.CategoryBases.Add(category);
			return this;
		}

		public TestCatalogBuilder WithProducts(int productCount, string categoryId = "", string propertySetId = "")
		{
			for (int index = 0; index < productCount; index++)
			{
				if (String.IsNullOrEmpty(propertySetId))
				{
					var catalog = GetCatalog() as VirtoCommerce.Foundation.Catalogs.Model.Catalog;
					propertySetId = catalog.PropertySets[0].PropertySetId;
				}

				if (String.IsNullOrEmpty(categoryId))
				{
					categoryId = _catalogBase.CategoryBases[0].CategoryId;
				}

				_items.Add(CreateProduct(_catalogBase.CatalogId, categoryId, "Product #" + index, propertySetId));
			}

			return this;
		}

		public TestCatalogBuilder ClearProducts()
		{
			_items.Clear();
			return this;
		}

		public CatalogBase GetCatalog()
		{
			return _catalogBase;
		}

		public Item[] GetItems()
		{
			return _items.ToArray();
		}

		private Category CreateCategory(string categoryName, string parentCategoryId)
		{
			Category category = new Category();
			category.Name = categoryName;
			category.StartDate = DateTime.Now.AddYears(-1);
			category.EndDate = DateTime.Now.AddYears(1);
			category.IsActive = true;
			//category.CatalogId = catalogName;
			//category.CategoryId = categoryId;
			//category.ParentCategoryId = parentCategoryId;
			category.Priority = 1;

			return category;

		}

		private static Product CreateProduct(string catalogId, string categoryId, string name, string propertySetId)
		{
			var product = new Product {Name = name};
			//product.CatalogId = catalogId;
			product.Code = product.ItemId;
			product.StartDate = DateTime.Now.AddYears(-1);
			product.EndDate = DateTime.Now.AddYears(1);
			product.IsActive = true;

			product.Weight = 2.3m;
			product.TrackInventory = false;
			//product.SetPropertyValue<string>("Brand", brand);

			string description =
				@"The Nokia 2610 is an easy to use device that combines multiple messaging options including email, instant messaging, and more. You can even download MP3 ringtones, graphics, and games straight to the phone, or surf the Internet with Cingular's MEdia Net service. It's the perfect complement to Cingular service for those even remotely interested in mobile Web capabilities in an affordable handset.";

			product.ItemPropertyValues.Add(new ItemPropertyValue()
				{
					ItemId = product.ItemId,
					LongTextValue = description,
					Name = "Description"
				});

			string design =
				"Compact and stylish, the 2610 features a candybar design sporting a bright 128 x 128 pixel display capable of displaying over 65,000 colors. Most of the phone's features and on-screen menus are controlled by a center toggle on the control pad. A standard hands-free headphone jack is provided, as are volume control keys, and there's even a \"Go-To\" button that can be assigned by the user for quick access to favorite applications. Lastly, the included speakerphone allows you to talk handsfree, and because the phone sports an internal antenna, there's nothing to snag or break off.";
			product.ItemPropertyValues.Add(new ItemPropertyValue()
				{
					ItemId = product.ItemId,
					LongTextValue = design,
					Name = "Design"
				});
			product.ItemPropertyValues.Add(new ItemPropertyValue()
				{
					ItemId = product.ItemId,
					LongTextValue = "2610",
					Name = "Model"
				});

			//var propertySet = catalog.PropertySets[0];

			//product.PropertySet = propertySet;
			product.PropertySetId = propertySetId;

			////Add associations
			//AssociationGroup group = new AssociationGroup();

			//group.Description = "Related products";
			//group.Name = "RelatedProducts";
			//group.ItemId = product.ItemId;

			//Association association = new Association();
			//association.AssociationType = AssociationTypes.Optional;
			//association.ItemId = product.ItemId;
			////association.CatalogItem = product;
			//association.AssociationGroupId = group.AssociationGroupId;
			//group.Associations.Add(association);

			//product.AssociationGroups.Add(group);

			//AssociationGroup group2 = new AssociationGroup();
			//group2.Description = "Accessory products";
			//group2.Name = "ProductAccessories";
			//group2.ItemId = product.ItemId;

			//Association association2 = new Association();
			//association2.AssociationType = AssociationTypes.Optional;
			//association2.ItemId = product.ItemId;
			////association2.CatalogItem = product;
			//association2.AssociationGroupId = group2.AssociationGroupId;
			//group2.Associations.Add(association2);

			//product.AssociationGroups.Add(group2);


			var asset = new ItemAsset
				{
					AssetId = String.Format("catalog/{0}", "image"),
					AssetType = "file",
					GroupName = "primaryimage",
					ItemId = product.ItemId,
					SortOrder = 1
				};

			product.ItemAssets.Add(asset);

			var relation = new CategoryItemRelation() {CatalogId = catalogId, CategoryId = categoryId, ItemId = product.ItemId};
			product.CategoryItemRelations.Add(relation);

			return product;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Services
{
	public class MockCatalogService : ICatalogService, ICatalogRepository, IPricelistRepository
	{
		private IList[] MockLists;

		private List<catalogModel.Catalog> CatalogList = new List<catalogModel.Catalog>();
		private List<catalogModel.Category> CategoryList = new List<catalogModel.Category>();
		private List<catalogModel.Product> ProductList = new List<catalogModel.Product>();
		private List<Property> AllProperties;
		private List<PropertySet> AllPropertySets;

		public MockCatalogService()
		{
			AllProperties = new Property[] {
                new Property { CatalogId = "Test Catalog", Name = "test", PropertyId = "Prop1", PropertyValueType=PropertyValueType.Integer.GetHashCode(), IsRequired=false, IsMultiValue = false },
                new Property { CatalogId = "Test Catalog", Name = "test1", PropertyId = "Prop2", PropertyValueType=PropertyValueType.Boolean.GetHashCode(), IsRequired=false, IsMultiValue = true },
                new Property { CatalogId = "Test Catalog", Name = "test2", PropertyId = "Prop3", PropertyValueType=PropertyValueType.Decimal.GetHashCode(), IsRequired=true, IsMultiValue = true },
                new Property { CatalogId = "Test Catalog", Name = "test3", PropertyId = "Prop", PropertyValueType=PropertyValueType.DateTime.GetHashCode(), IsRequired=true, IsMultiValue = true },
                // new Property { CatalogId = "Test Catalog", Name = "test4", PropertyId = "Prop4", PropertyValueType=PropertyValueType.DictionaryKey.GetHashCode(), IsRequired=false },
                new Property { CatalogId = "Test Catalog", Name = "test5", PropertyId = "Prop5", PropertyValueType=PropertyValueType.Image.GetHashCode(), IsRequired=false },
                new Property { CatalogId = "Test Catalog", Name = "test6", PropertyId = "Prop6", PropertyValueType=PropertyValueType.LongString.GetHashCode()},
                new Property { CatalogId = "Test Catalog", Name = "test7", PropertyId = "Prop7", PropertyValueType=PropertyValueType.ShortString.GetHashCode(), IsRequired=true, IsMultiValue = true }}
			.ToList();

			var enumProperty = new Property { CatalogId = "Test Catalog", Name = "enum 0", PropertyId = "enum 0", PropertyValueType = PropertyValueType.Integer.GetHashCode(), IsRequired = true, IsEnum = true };
			enumProperty.PropertyValues.Add(new PropertyValue { PropertyValueId = "int1", IntegerValue = 11, ValueType = PropertyValueType.Integer.GetHashCode() });
			enumProperty.PropertyValues.Add(new PropertyValue { PropertyValueId = "int2", IntegerValue = 722, ValueType = PropertyValueType.Integer.GetHashCode() });
			enumProperty.PropertyValues.Add(new PropertyValue { PropertyValueId = "int3", IntegerValue = 34, ValueType = PropertyValueType.Integer.GetHashCode() });
			AllProperties.Add(enumProperty);

			AllProperties[0].PropertyValues.Add(new PropertyValue { IntegerValue = 751, BooleanValue = true, ValueType = PropertyValueType.Integer.GetHashCode() });
			AllProperties[0].PropertyValues.Add(new PropertyValue { BooleanValue = false, ValueType = PropertyValueType.Boolean.GetHashCode() });

			AllProperties[1].PropertyValues.Add(new PropertyValue { BooleanValue = true, ValueType = PropertyValueType.Boolean.GetHashCode() });

			AllProperties[1].PropertyAttributes.Add(new PropertyAttribute { PropertyAttributeName = "IndexStore", PropertyAttributeValue = "Stored" });
			AllProperties[2].PropertyAttributes.Add(new PropertyAttribute { PropertyAttributeName = "IndexStore", PropertyAttributeValue = "Stored" });

			CatalogList.Add(new catalogModel.Catalog() { Name = "Test Catalog", DefaultLanguage = "ca", CatalogId = "Test Catalog" });
			CatalogList[0].CatalogLanguages.Add(new CatalogLanguage { CatalogId = CatalogList[0].CatalogId, Language = "ca" });
			CatalogList[0].CatalogLanguages.Add(new CatalogLanguage { CatalogId = CatalogList[0].CatalogId, Language = "en-us" });

			AllPropertySets = new List<PropertySet>(new PropertySet[]{
                new PropertySet { Name = "set1", PropertySetId = "id1", TargetType = PropertyTargetType.Product.ToString() },
                new PropertySet { Name = "set2", PropertySetId = "id2", TargetType = PropertyTargetType.Category.ToString() },
                new PropertySet { Name = "set3", PropertySetId = "id3", TargetType = PropertyTargetType.All.ToString() }
            });

			var enumPropertySet = new PropertySet { Name = "enum set1", PropertySetId = "enumid1", TargetType = PropertyTargetType.All.ToString() };
			enumPropertySet.PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "enum11", Priority = 1, Property = enumProperty });
			enumPropertySet.PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "enum12", Property = AllProperties[0] });
			enumPropertySet.PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "enum13", Property = AllProperties[4] });
			AllPropertySets.Add(enumPropertySet);

			CatalogList[0].PropertySets.Add(AllPropertySets);

			CatalogList[0].PropertySets[0].PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "id00", Priority = 1, Property = AllProperties[2] });
			CatalogList[0].PropertySets[0].PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "id01", Priority = 5, Property = AllProperties[0] });

			CatalogList[0].PropertySets[1].PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "id10", Priority = 2, Property = AllProperties[2] });
			CatalogList[0].PropertySets[1].PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "id11", Priority = 0, Property = AllProperties[0] });
			CatalogList[0].PropertySets[1].PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "id11", Priority = 1, Property = AllProperties[1] });

			for (int i = 0; i < AllProperties.Count; i++)
			{
				CatalogList[0].PropertySets[2].PropertySetProperties.Add(new PropertySetProperty { PropertySetId = "id2" + i, Priority = 1 + i, Property = AllProperties[i] });
			}

			CatalogList[0].PropertySets.ToList().ForEach(y => y.PropertySetProperties.ToList().ForEach(x => x.PropertyId = x.Property.PropertyId));

			CategoryList.Add(new Category() { CategoryId = "furniture", Name = "Furniture" });
			CategoryList.Add(new Category() { CategoryId = "living_room", Name = "Living Room", ParentCategoryId = "furniture" });
			CategoryList.Add(new Category() { CategoryId = "living_room", Name = "Living Room", ParentCategoryId = "furniture" });

			CategoryList.Add(new Category() { CategoryId = "electronics", Name = "Electronics" });
			CategoryList.Add(new Category() { CategoryId = "cell_phones", Name = "Cell Phones", ParentCategoryId = "electronics" });
			CategoryList.Add(new Category() { CategoryId = "cameras", Name = "Cameras", ParentCategoryId = "electronics" });
			CategoryList.Add(new Category() { CategoryId = "accessories", Name = "Accessories", ParentCategoryId = "electronics" });
			CategoryList.Add(new Category() { CategoryId = "digital_cameras", Name = "Digital Cameras", ParentCategoryId = "electronics" });
			CategoryList.Add(new Category() { CategoryId = "computers", Name = "Computers", ParentCategoryId = "electronics" });

			CategoryList.Add(new Category() { CategoryId = "apparel", Name = "Apparel" });
			CategoryList.Add(new Category() { CategoryId = "shirts", Name = "Shirts", ParentCategoryId = "apparel" });
			CategoryList.Add(new Category() { CategoryId = "shoes", Name = "Shoes", ParentCategoryId = "apparel" });
			CategoryList.Add(new Category() { CategoryId = "mens", Name = "Mens", ParentCategoryId = "apparel" });
			CategoryList.Add(new Category() { CategoryId = "womens", Name = "Womens", ParentCategoryId = "apparel" });
			CategoryList.Add(new Category() { CategoryId = "hoodies", Name = "Hoodies", ParentCategoryId = "apparel" });

			CategoryList.Add(new Category() { CategoryId = "music", Name = "Music" });
			CategoryList.Add(new Category() { CategoryId = "ebooks", Name = "Ebooks" });

			for (int i = 0; i < CategoryList.Count; i++)
			{
				CategoryList[i].PropertySet = CatalogList[0].PropertySets[1 + ((2 + i) % 3)];
				CategoryList[i].PropertySetId = CategoryList[i].PropertySet.PropertySetId;
			}

			// how to bind to property??? - by 'Name
			CategoryList[0].CategoryPropertyValues.Add(new CategoryPropertyValue { Name = "enum 0", KeyValue = "int3", });
			CategoryList[0].CategoryPropertyValues.Add(new CategoryPropertyValue { Name = "test0", IntegerValue = 456, ValueType = PropertyValueType.Integer.GetHashCode() });

			ProductList.Add(new Product() { ItemId = "chair", Name = "Chair", PropertySetId = "enumid1", Weight = 10.32m, MaxQuantity = 20, MinQuantity = 1 });
			ProductList.Add(new Product() { ItemId = "bed", Name = "Bed", PropertySetId = "id1", Weight = 13.12m, MaxQuantity = 26, MinQuantity = 3 });
			ProductList.Add(new Product() { ItemId = "iphone", Name = "Apple iPhone", PropertySetId = "id2", Weight = 1.53m, MaxQuantity = 1000, MinQuantity = 5 });
			ProductList.Add(new Product() { ItemId = "canon_powershot", Name = "Canon Powershot", PropertySetId = "id3", Weight = 6.12m, MaxQuantity = 40, MinQuantity = 2 });
			ProductList.Add(new Product() { ItemId = "sonycharger", Name = "Sony Charger", PropertySetId = "enumid1", Weight = 16.32m, MaxQuantity = 60, MinQuantity = 4 });
			ProductList.Add(new Product() { ItemId = "kodakcamera", Name = "Kodak Digital Camera", PropertySetId = "id2", Weight = 89.32m, MaxQuantity = 2, MinQuantity = 1 });
			ProductList.Add(new Product() { ItemId = "hpcomputer", Name = "HP Computer", PropertySetId = "id1", Weight = 1.42m, MaxQuantity = 12, MinQuantity = 1 });

			ProductList[0].ItemAssets.Add(new ItemAsset { AssetId = "aid01", AssetType = "image", ItemAssetId = "http://virtoamazon.blob.core.windows.net/catalog/v-b0009m0nfq/ajsl6jl0p0wabps8xjboiw.jpg" });

			ProductList[4].ItemAssets.Add(new ItemAsset { AssetId = "aid02", AssetType = "image", ItemAssetId = "http://virtoamazon.blob.core.windows.net/catalog/v-b001e27lpa/lr5pdabuneqdnwecsgumhg.jpg" });
			ProductList[4].ItemAssets.Add(new ItemAsset { AssetId = "aid03", AssetType = "image", ItemAssetId = "http://virtoamazon.blob.core.windows.net/catalog/v-b00001w0h1/zn1nm-6z-emdd9ts_170sa.jpg" });

			ProductList[1].ItemPropertyValues.Add(new ItemPropertyValue { ValueType = PropertyValueType.Integer.GetHashCode(), Name = "test0", IntegerValue = 123 });


			AllPropertySets.ForEach((x) => x.CatalogId = CatalogList[0].CatalogId);
			CategoryList.ForEach((x) => { x.CatalogId = CatalogList[0].CatalogId; x.Code = x.CategoryId; });
			ProductList.ForEach((x) => x.CatalogId = CatalogList[0].CatalogId);

			MockLists = new IList[] { CatalogList, CategoryList, AllProperties, AllPropertySets, ProductList };
			// MockLists.Add(CatalogList.OfType<StorageEntity>().ToList());

		}

		#region ICatalogService Members

		public CatalogBase[] GetAllCatalogs()
		{
			return CatalogList.ToArray();
		}

		public CategoryBase[] GetAllChildCategories(string catalogId, string parentCategoryId)
		{
			return CategoryList.Where(x => x.CatalogId == catalogId && (x.ParentCategoryId == parentCategoryId)).ToArray();
		}

		public Item[] GetItemsByIds(string[] ids)
		{
			return ProductList.ToArray();
		}

		public VirtoCommerce.Foundation.Catalogs.Search.CatalogItemSearchResults SearchItems(string scope, VirtoCommerce.Foundation.Catalogs.Search.CatalogItemSearchCriteria criteria)
		{
			var resultDocumentSet = new ResultDocumentSet();
			resultDocumentSet.TotalCount = ProductList.Count();
			resultDocumentSet.Documents = new ResultDocument[] { };

			var searchResult = new SearchResults(criteria, new ResultDocumentSet[] { resultDocumentSet });
			var items = ProductList.Skip(criteria.StartingRecord).Take(criteria.RecordsToRetrieve).ToArray();
			var result = new CatalogItemSearchResults(criteria, null, searchResult);

			return result;
		}

		public Pricelist[] GetAllPriceLists(bool returnPrices, bool useCache)
		{
			Pricelist[] result = new Pricelist[] {
                new Pricelist { PricelistId = "premium" } ,
                new Pricelist { PricelistId = "default" } ,
                new Pricelist { PricelistId = "sale1" }
            };

			result[1].Prices.Add(new Price() { ItemId = "chair", List = 75.1m });
			result[1].Prices.Add(new Price() { ItemId = "bed", List = 125m });
			result[1].Prices.Add(new Price() { ItemId = "iphone", List = 175m });
			result[1].Prices.Add(new Price() { ItemId = "canon_powershot", List = 105m });
			result[1].Prices.Add(new Price() { ItemId = "sonycharger", List = 12m });
			result[1].Prices.Add(new Price() { ItemId = "kodakcamera", List = 125.99m });
			result[1].Prices.Add(new Price() { ItemId = "hpcomputer", List = 1025m });

			return result;
		}

		#endregion

		#region ICatalogRepository Members

		public IQueryable<CategoryBase> Categories
		{
			get { return CategoryList.AsQueryable(); }
		}

		public IQueryable<CatalogBase> Catalogs
		{
			get { return CatalogList.AsQueryable(); }
		}

		public IQueryable<Item> Items
		{
			get { return ProductList.AsQueryable(); }
		}

		public IQueryable<Product> Products
		{
			get { return ProductList.AsQueryable().OfType<Product>(); }
		}

		public IQueryable<Property> Properties
		{
			get { return AllProperties.AsQueryable(); }
		}

		public IQueryable<PropertySet> PropertySets
		{
			get { return AllPropertySets.AsQueryable(); }
		}

		public IQueryable<ItemRelation> ItemRelations
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<Association> Associations
		{
			get { throw new NotImplementedException(); }
		}
		#endregion

		#region IRepository Members

		MockUnitOfWork MockUnitOfWorkItem = new MockUnitOfWork();
		public IUnitOfWork UnitOfWork
		{
			get { return MockUnitOfWorkItem; }
		}

		public void Attach<T>(T item) where T : class
		{

		}

		public void Add<T>(T item) where T : class
		{
			var list = MockLists.OfType<List<T>>().First();

			if (!list.Contains(item))
				list.Add(item);
		}

		public void Update<T>(T item) where T : class
		{
			//var list = MockLists.OfType<List<T>>().First();

			//if (list.Contains(item))
			//{
			//}
			//else
			//{
			//    list.Where(x => x.)
			//    list.Add(item);
			//}
		}

		public void Remove<T>(T item) where T : class
		{
			var list = MockLists.OfType<List<T>>().First();
			list.Remove(item);
		}

		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			throw new NotImplementedException();
		}

	    public void Refresh(IEnumerable collection)
	    {
	        throw new NotImplementedException();
	    }

	    public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }


		#endregion

		#region IPriceListRepository Members

		public IQueryable<Pricelist> Pricelists
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<Price> Prices
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<PricelistAssignment> PricelistAssignments
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{

		}

		#endregion

		#region ICatalogRepository Members


		public IQueryable<CategoryItemRelation> CategoryItemRelations
		{
			get { throw new NotImplementedException(); }
		}

		#endregion


		public IQueryable<Packaging> Packagings
		{
			get { throw new NotImplementedException(); }
		}

		public IQueryable<TaxCategory> TaxCategories
		{
			get { throw new NotImplementedException(); }
		}
	}

	public class MockUnitOfWork : IUnitOfWork
	{
		public int Commit()
		{
			return 0;
		}

		public void CommitAndRefreshChanges()
		{
		}

		public void RollbackChanges()
		{
		}
	}
}

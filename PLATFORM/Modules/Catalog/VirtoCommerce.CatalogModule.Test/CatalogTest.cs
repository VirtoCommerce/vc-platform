using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using System.Collections.Generic;
using module = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Test
{
	[TestClass]
	public class CatalogTest
	{
		[TestMethod]
		public void CatalogPatchTest()
		{
			var chageTracker = new ObservableChangeTracker
			{
				RemoveAction = (x) =>
				{
					Console.WriteLine(x.ToString());
				}
			};

			var dbCatalog1 = new dataModel.Catalog
			{
				 Name = "catalog1",
				 DefaultLanguage = "en-us"
			};
			dbCatalog1.CatalogLanguages.Add(new dataModel.CatalogLanguage { Language = "en-us" });
			dbCatalog1.CatalogLanguages.Add(new dataModel.CatalogLanguage { Language = "fr-fr" });

			var dbCatalog2 = new dataModel.Catalog
			{
				Name = "unknow"
			};
			dbCatalog2.CatalogLanguages.Add(new dataModel.CatalogLanguage { Language = "ru-ru" });

			chageTracker.Attach(dbCatalog2);
			dbCatalog1.Patch(dbCatalog2);

		}

		[TestMethod]
		public void WorkingWithCatalogTest()
		{
			var catalogService = GetCatalogService();
			var catalog = new Catalog
			{
				Name = "Test",
			};
			var languages = new CatalogLanguage[]
			{
				new CatalogLanguage { LanguageCode = "en-us"}
			};
			//Create catalog
			catalog.Languages = languages.ToList();
			catalog = catalogService.Create(catalog);
			//Add language
			catalog.Languages.Add( new  CatalogLanguage { LanguageCode = "fr-fr", IsDefault = true });
			catalog.Name = null; //not define should no changed
			catalogService.Update(new Catalog[] { catalog });
			//Clear languages
			catalog.Languages.Clear();
			catalogService.Update(new Catalog[] { catalog });
			//Verification
			catalog = catalogService.GetById(catalog.Id);

			Assert.IsTrue(catalog.Name == "Test");
			Assert.IsFalse(catalog.Languages.Any());
		}

		[TestMethod]
		public void GetProduct()
		{
			var itemService = GetItemService();
			var product = itemService.GetById("v-b004nzb8tu", ItemResponseGroup.ItemLarge);
		}

		[TestMethod]
		public void WorkingWithItemTest()
		{
			var itemService = GetItemService();
			var catalogService = GetCatalogService();
			var categoryService = GetCategoryService();
			var propertyService = GetPropertyService();

			var category = categoryService.GetById("a8bfc7cd-4363-4f12-976e-3f236433b6cf");

			//Add product property to category
			//var productProperty = new Property
			//{
			//	CategoryId = category.Id,
			//	CatalogId = category.Catalog.Id,
			//	Name = "testProp",
			//	Type = PropertyType.Product,
			//	ValueType = PropertyValueType.ShortText
			//};
			//productProperty = propertyService.Create(productProperty);


			var productProperty = propertyService.GetCategoryProperties(category.Id)
													.Where(x=>x.Type == PropertyType.Product).FirstOrDefault();
			var productPropValue = new PropertyValue
			{
				//Property = productProperty,
				PropertyId = productProperty.Id,
				Value = "some value",
				PropertyName = productProperty.Name
			};
			var product = new CatalogProduct
			{
				Name = "TestProduct",
				Code = "Code",
				CatalogId = category.CatalogId,
				CategoryId = category.Id
			};
			product.PropertyValues = new List<PropertyValue>();
			product.PropertyValues.Add(productPropValue);
			product = itemService.Create(product);
		}

		[TestMethod]
		public void CreateProperty()
		{
			var categoryService = GetCategoryService();
			var propertyService = GetPropertyService();
			var category = categoryService.GetById("a8bfc7cd-4363-4f12-976e-3f236433b6cf");
			var property = new Property
			{
				Id = "testProperty",
				CatalogId = category.CatalogId,
				CategoryId = category.Id,
				Name = "testProperty2",
				Type = PropertyType.Product,
				ValueType = PropertyValueType.ShortText,
				Dictionary = true
			};
			property.Attributes = new List<PropertyAttribute>();
			property.DictionaryValues = new List<PropertyDictionaryValue>();

			var attribute = new PropertyAttribute
			{
				 Name = "attr1",
				 Value = "val1"

			};
			property.Attributes.Add(attribute);

			property.DictionaryValues.Add(new PropertyDictionaryValue { Value = "ss", Property = property });
			property.DictionaryValues.Add(new PropertyDictionaryValue { Value = "ddd", Property = property });

			//property = propertyService.Create(property);

			property = propertyService.GetById(property.Id);

			property.DictionaryValues.Remove(property.DictionaryValues.First());
			property.DictionaryValues.Add(new PropertyDictionaryValue { Value = "fff", Property = property });

			propertyService.Update(new Property[] { property });

			property = propertyService.GetById(property.Id);

			propertyService.Delete(new string[] { property.Id });
		}

		[TestMethod]
		public void SearchTest()
		{
			var criteria = new SearchCriteria()
			{
				ResponseGroup = ResponseGroup.WithCatalogs | ResponseGroup.WithCategories
			};
			var searchService = GetSearchService();

			var result = searchService.Search(criteria);

			criteria.CatalogId = "Samsung";
			
			result = searchService.Search(criteria);

			criteria.ResponseGroup = ResponseGroup.WithProducts;
			criteria.CategoryId = result.Categories[0].Id;

			result = searchService.Search(criteria);
		}

		[TestMethod]
		public void AddLinkToCatalog()
		{
			var catService = GetCatalogService();
			var categoryService = GetCategoryService();
			var itemService = GetItemService();
			var searchService = GetSearchService();
			//Create virtual catalog
			var vCatalog = new Catalog
			{
				Id = "vCat",
				Name = "vCat",
				Virtual = true
			};
			//vCatalog = catService.Create(vCatalog);

			var category = categoryService.GetById("03771c0e-51ac-44d0-ac77-2a38b56b11b5");
			category.Links.Add(new CategoryLink { CatalogId = vCatalog.Id });
			//categoryService.Update(new Category[] { category });

			var item = itemService.GetById("v-b0007pn5n2", ItemResponseGroup.ItemLarge);
			item.Links.Add(new CategoryLink { CatalogId = vCatalog.Id });
			itemService.Update(new CatalogProduct[] { item });
		}

		[TestMethod]
		public void VirtualCategories()
		{
			var catService = GetCatalogService();
			var categoryService = GetCategoryService();
			var itemService = GetItemService();
			var searchService = GetSearchService();

			var catalog = new Catalog
			{
				Id = "Cat",
				Name = "Cat",
			};
			catalog = catService.Create(catalog);
			var category = new Category
			{
				Id = "Category",
				CatalogId = catalog.Id,
				Name = "Category",
				Code = "Category"
			};
			category = categoryService.Create(category);

			//Create virtual catalog
			var vCatalog = new Catalog
			{
				Id = "vCat",
				Name = "vCat",
				Virtual = true
			};
			vCatalog = catService.Create(vCatalog);
			var vCategory = new Category
			{
				Id = "vCategory",
				CatalogId = vCatalog.Id,
				Name = "vCategory",
				Code = "vCategory",
				Virtual = true
			};
			vCategory = categoryService.Create(vCategory);

			vCategory = categoryService.GetById(vCategory.Id);
			vCategory.Links.Add(new CategoryLink { CatalogId = catalog.Id, CategoryId = category.Id });
			categoryService.Update(new Category[] { vCategory });

			category = categoryService.GetById(category.Id);
			category.Name = "category111";
			categoryService.Update(new Category[] { category });

			Assert.IsTrue(category.Links.First().CategoryId == "vCategory");
			Assert.IsTrue(category.Links.First().CatalogId == "vCat");

			vCategory = categoryService.GetById(vCategory.Id);
			Assert.IsTrue(vCategory.Links.First().CategoryId == "Category");
			Assert.IsTrue(vCategory.Links.First().CatalogId == "Cat");


			//add link product to virtual category
			var product = itemService.GetById("v-b002c7481g", ItemResponseGroup.ItemLarge);
			product.Links.Add(new CategoryLink { CatalogId = vCatalog.Id, CategoryId = vCategory.Id });
			itemService.Update(new CatalogProduct[] { product });
			product = itemService.GetById("v-b002c7481g", ItemResponseGroup.ItemLarge);
			Assert.IsTrue(product.Links.Count() == 2);

			//Check search 
			var result = searchService.Search(new SearchCriteria { CatalogId = vCatalog.Id, CategoryId = vCategory.Id, ResponseGroup = ResponseGroup.WithProducts });
			Assert.IsTrue(result.Products.Any(x => x.Id == product.Id));

			//Remove link
			product.Links.Remove(product.Links.First(x => x.CategoryId == vCategory.Id));
			itemService.Update(new CatalogProduct[] { product });
			product = itemService.GetById("v-b002c7481g", ItemResponseGroup.ItemLarge);
			Assert.IsTrue(product.Links.Count() == 1);

			vCategory.Links.Clear();
			categoryService.Update(new Category[] { vCategory });

			category = categoryService.GetById(category.Id);
			Assert.IsFalse(category.Links.Any());
			Assert.IsFalse(category.Links.Any());

			vCategory = categoryService.GetById(vCategory.Id);
			Assert.IsFalse(vCategory.Links.Any());
			Assert.IsFalse(vCategory.Links.Any());

		}

		[TestMethod]
		public void VirtualCatalogsTest()
		{
			var searchService = GetSearchService();
			var result = searchService.Search(new SearchCriteria { CategoryId = "vCategory", CatalogId="vCat", ResponseGroup = ResponseGroup.WithCategories});
		}



		private ICatalogSearchService GetSearchService()
		{
			return new CatalogSearchServiceImpl(GetRepository, GetItemService(), GetCatalogService(), GetCategoryService());
		}

		private IPropertyService GetPropertyService()
		{
			return new PropertyServiceImpl(() => { return GetRepository(); });
		}

		private ICategoryService GetCategoryService()
		{
			return new CategoryServiceImpl(() => { return GetRepository(); });
		}

		private ICatalogService GetCatalogService()
		{
			return new CatalogServiceImpl(() => { return GetRepository(); });
		}

		private IItemService GetItemService()
		{
			return new ItemServiceImpl(() => { return GetRepository(); });
		}


		private ICatalogRepository GetRepository()
		{
			var retVal = new CatalogRepositoryImpl("VirtoCommerce");
			return retVal;
		}
	
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CsvHelper;
using System.Linq;
using CsvHelper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Commerce.Services;
using Omu.ValueInjecter;
using VirtoCommerce.CoreModule.Data.Repositories;
using System.Dynamic;
namespace VirtoCommerce.CatalogModule.Test
{
	[TestClass]
	public class ImportExportTest
	{
		[TestMethod]
		public void ExportProductsTest()
		{
			var searchService = GetSearchService();
			var categoryService = GetCategoryService();
			var itemService = GetItemService();
			var result = searchService.Search(new SearchCriteria { CatalogId = "Sony", CategoryId = "66b58f4c-fd62-4c17-ab3b-2fb22e82704a", Start = 0, Count = 10 , ResponseGroup = ResponseGroup.WithProducts });
		
			using (var csvWriter = new CsvWriter(new StreamWriter(@"c:\Projects\VCF\vc-community\PLATFORM\Modules\Catalog\VirtoCommerce.CatalogModule.Test\products.csv")))
			{
				csvWriter.Configuration.Delimiter = ";";

				var fullLoadedProducts = new List<coreModel.CatalogProduct>();
				foreach (var product in result.Products)
				{
					var fullLoadedProduct = itemService.GetById(product.Id, ItemResponseGroup.ItemLarge);
					fullLoadedProducts.Add(fullLoadedProduct);
				}

				//Write header
				var headers = new string[] { "Name", "Code", "Category", "Reviews" }.Concat(fullLoadedProducts.SelectMany(x=>x.PropertyValues.Select(y=>y.PropertyName)).Distinct()).ToArray();
				foreach (var header in headers)
				{
					csvWriter.WriteField(header);
				}


				csvWriter.NextRecord();
				
				foreach (var fullLoadedProduct in fullLoadedProducts)
				{
					var propertyInfos = fullLoadedProduct.GetProps();
					foreach (var header in headers)
					{
						var propertyInfo = propertyInfos.Find(header, true); 
						if(header == "Reviews")
						{
							var review = fullLoadedProduct.Reviews.FirstOrDefault();
							csvWriter.WriteField(review != null ? review.Content : String.Empty);
						}
						else if(header == "Category")
						{
							var category = categoryService.GetById(fullLoadedProduct.CategoryId);
							var path = String.Join("/", category.Parents.Select(x => x.Name).Concat(new string[] { category .Name }));
							csvWriter.WriteField(path);
						}
						else if(propertyInfo != null)
						{
							var propValue = propertyInfo.GetValue(fullLoadedProduct);
							csvWriter.WriteField(propValue ?? String.Empty);
						}
						else
						{
							var propertyValue = fullLoadedProduct.PropertyValues.FirstOrDefault(x=>x.PropertyName == header);
							csvWriter.WriteField(propertyValue != null ? (propertyValue.Value ?? String.Empty) : String.Empty);
						}
					
					}
				
					csvWriter.NextRecord();
				}
			}

		}


		[TestMethod]
		public void ImportProductsTest()
		{
			var catalogId = "57b2ed0c42a94eb88f1c7b97afaf183d";

			var product = new coreModel.CatalogProduct();
			//Auto detect mapping configuration
			var mappingConfiguration = new string[] { "Name", "Code", "Category", "Reviews" }.Select(x => new webModel.CsvImportMappingItem { EntityColumnName = x }).ToArray();
			DoAutoMapping(mappingConfiguration);
			//Edit mapping configuration in UI

			var reviewMappingItem = mappingConfiguration.First(x => x.EntityColumnName == "Reviews");
			reviewMappingItem.CsvColumnName = "Reviews";
			//Start import
			//read objects from csv use mapping configuration
			var csvProducts = new List<CatalogProduct>();
			using (var reader = new CsvReader(new StreamReader(@"c:\Projects\VCF\vc-community\PLATFORM\Modules\Catalog\VirtoCommerce.CatalogModule.Test\products.csv")))
			{
				reader.Configuration.Delimiter = ";";
				var initialized = false;
				while (reader.Read())
				{
					if (!initialized)
					{
						var productMap = new ProductMap(reader.FieldHeaders, mappingConfiguration);
						reader.Configuration.RegisterClassMap(productMap);
						initialized = true;
					}

					var csvProduct = reader.GetRecord<coreModel.CatalogProduct>();
					csvProducts.Add(csvProduct);
				}
			};
			
			var categories = new List<coreModel.Category>();
			//project product information to category structure (categories, properties etc)
			foreach (var csvProduct in csvProducts)
			{
				var productCategoryNames = csvProduct.Category.Path.Split('/');
				ICollection<coreModel.Category> levelCategories = categories;
				foreach (var categoryName in productCategoryNames)
				{
					var category = levelCategories.FirstOrDefault(x => x.Name == categoryName);
					if(category == null)
					{
						category = new coreModel.Category() { Name = categoryName, Code = categoryName.GenerateSlug() };
						category.CatalogId = catalogId;
						category.Children = new List<coreModel.Category>();
						levelCategories.Add(category);
					}
					csvProduct.Category = category;
					levelCategories = category.Children;
				}
			}

			var categoryService = GetCategoryService();
			var newCategories = new List<coreModel.Category>();
			//save to db
			//Categories
 			foreach(var category in categories)
			{
				var newCategory = categoryService.Create(category);
				newCategories.Add(newCategory);
				foreach(var childCategory in category.Traverse(x=>x.Children))
				{
					newCategory = categoryService.Create(childCategory);
					newCategories.Add(newCategory); 
				}
			}
			var productService = GetItemService();
	
			//Products
			foreach (var csvProduct in csvProducts)
			{
				var sameProduct =  csvProducts.FirstOrDefault(x=>x.Name == csvProduct.Name && !x.IsTransient());
				if(sameProduct != null)
				{
					//Detect variation
					csvProduct.MainProductId = sameProduct.Id;
				}
				var category = newCategories.FirstOrDefault(x => x.Code == csvProduct.Category.Code);
				csvProduct.CategoryId = category.Id;
				csvProduct.CatalogId = catalogId;
				var newProduct = productService.Create(csvProduct);
				csvProduct.Id = newProduct.Id;
			}

			
		}

		private void DoAutoMapping(webModel.CsvImportMappingItem[] mappingItems)
		{
			using (var reader = new CsvReader(new StreamReader(@"c:\Projects\VCF\vc-community\PLATFORM\Modules\Catalog\VirtoCommerce.CatalogModule.Test\products.csv")))
			{
				reader.Configuration.Delimiter = ";";
				while (reader.Read())
				{
					var csvColumns = reader.FieldHeaders;

					//default columns mapping
					if (csvColumns.Any())
					{
						foreach (var csvColumn in csvColumns)
						{
							var mappingItem = mappingItems.FirstOrDefault(x => x.EntityColumnName.ToLower().Contains(csvColumn.ToLower()) ||
																	csvColumn.ToLower().Contains(x.EntityColumnName.ToLower()));
							//if entity column name contains csv column name or visa versa - match entity property name to csv file column name
							if (mappingItem != null)
							{
								mappingItem.CsvColumnName = csvColumn;
								mappingItem.CustomValue = null;
							}
						}
					}
				}
			}

		}

		public sealed class ProductMap : CsvClassMap<coreModel.CatalogProduct>
		{
			public ProductMap(string[] allColumns, webModel.CsvImportMappingItem[]  mappingConfiguration)
			{
				var attributePropertyNames = allColumns.Except(mappingConfiguration.Select(x=>x.CsvColumnName));
				foreach(var mappingConfigurationItem in mappingConfiguration)
				{
					var propertyInfo = typeof(coreModel.CatalogProduct).GetProperty(mappingConfigurationItem.EntityColumnName);
					var newMap = new CsvPropertyMap(propertyInfo);
					newMap.Name(mappingConfigurationItem.CsvColumnName);
					PropertyMaps.Add(newMap);
				}
				var categoryMappingItem = mappingConfiguration.First(x => x.EntityColumnName == "Category");
				var editorialReviewMappingItem = mappingConfiguration.First(x => x.EntityColumnName == "Reviews");
				Map(x => x.Category).ConvertUsing(x => new coreModel.Category { Path = x.GetField<string>(categoryMappingItem.CsvColumnName) });
				Map(x => x.Reviews).ConvertUsing(x => new coreModel.EditorialReview[] { new coreModel.EditorialReview { Content = x.GetField<string>(editorialReviewMappingItem.CsvColumnName) } });
				Map(x => x.PropertyValues).ConvertUsing(x => attributePropertyNames.Select(column => new coreModel.PropertyValue { PropertyName = column, Value = x.GetField<string>(column) }).ToList());

			}
		
		}

		private ICatalogSearchService GetSearchService()
		{
			return new CatalogSearchServiceImpl(GetRepository, GetItemService(), GetCatalogService(), GetCategoryService(), GetCommerceService());
		}

		private ICategoryService GetCategoryService()
		{
			return new CategoryServiceImpl(() => { return GetRepository(); }, GetCommerceService());
		}

		private ICatalogService GetCatalogService()
		{
			return new CatalogServiceImpl(() => { return GetRepository(); }, GetCommerceService());
		}

		private IItemService GetItemService()
		{
			return new ItemServiceImpl(() => { return GetRepository(); }, GetCommerceService());
		}

		private ICommerceService GetCommerceService()
		{
			return new CommerceServiceImpl(() => new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor()));
		}
		private ICatalogRepository GetRepository()
		{
			var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
			return retVal;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Common;
using System.IO;
using CsvHelper;
using System.Text;
using System.Reflection;
using VirtoCommerce.CatalogModule.Web.Model.Notifications;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using CsvHelper.Configuration;


namespace VirtoCommerce.CatalogModule.Web.BackgroundJobs
{
	public class CsvCatalogImportJob
	{
		private readonly ICatalogService _catalogService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _productService;
		private readonly INotifier _notifier;
		private readonly CacheManager _cacheManager;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly ISkuGenerator _skuGenerator;

		public CsvCatalogImportJob(ICatalogService catalogService, ICategoryService categoryService, IItemService productService,
								INotifier notifier, CacheManager cacheManager, IBlobStorageProvider blobProvider, ISkuGenerator skuGenerator)
		{
			_catalogService = catalogService;
			_categoryService = categoryService;
			_productService = productService;
			_notifier = notifier;
			_cacheManager = cacheManager;
			_blobStorageProvider = blobProvider;
			_skuGenerator = skuGenerator;
		}

		public virtual void DoImport(webModel.CsvImportConfiguration configuration, ImportNotification notification)
		{
			var csvProducts = new List<coreModel.CatalogProduct>();
			var catalog = _catalogService.GetById(configuration.CatalogId);
			try
			{
				using (var reader = new CsvReader(new StreamReader(_blobStorageProvider.OpenReadOnly(configuration.FileUrl))))
				{

					reader.Configuration.Delimiter = configuration.Delimiter;
					var initialized = false;
					while (reader.Read())
					{
						if (!initialized)
						{
							//Notification
							notification.Description = "Configuration...";
							_notifier.Upsert(notification);

							//set import configuration based on mapping info
							var productMap = new ProductMap(reader.FieldHeaders, configuration, catalog);
							reader.Configuration.RegisterClassMap(productMap);
							initialized = true;
						}

						//Notification
						notification.Description = "Reading products from csv...";
						_notifier.Upsert(notification);

						var csvProduct = reader.GetRecord<coreModel.CatalogProduct>();
						csvProducts.Add(csvProduct);
					}
				}

				SaveProducts(catalog, csvProducts, notification);
			}
			catch (Exception ex)
			{
				notification.Description = "Export error";
				notification.ErrorCount++;
				notification.Errors.Add(ex.ToString());
			}
			finally
			{
				notification.Finished = DateTime.UtcNow;
				_notifier.Upsert(notification);
			}


		}

		private void SaveProducts(coreModel.Catalog catalog, IEnumerable<coreModel.CatalogProduct> csvProducts, ImportNotification notification)
		{
			var categories = new List<coreModel.Category>();

			//project product information to category structure (categories, properties etc)
			foreach (var csvProduct in csvProducts)
			{
				var productCategoryNames = csvProduct.Category.Path.Split('/');
				ICollection<coreModel.Category> levelCategories = categories;
				foreach (var categoryName in productCategoryNames)
				{
					var category = levelCategories.FirstOrDefault(x => x.Name == categoryName);
					if (category == null)
					{
						category = new coreModel.Category() { Name = categoryName, Code = categoryName.GenerateSlug() };
						category.CatalogId = catalog.Id;
						category.Children = new List<coreModel.Category>();
						levelCategories.Add(category);
					}
					csvProduct.Category = category;
					levelCategories = category.Children;
				}
			}

			var newCategories = new List<coreModel.Category>();
			var counter = 0;
			//save to db
			//Categories
			notification.TotalCount = categories.SelectMany(x => x.Traverse(y => y.Children)).Count();
			foreach (var category in categories)
			{
				SaveCategoryRecursive(newCategories, category, notification);
			}

			var notifyProductSizeLimit = 10;
			notification.TotalCount = csvProducts.Count();
			//Products
			foreach (var csvProduct in csvProducts)
			{
				var sameProduct = csvProducts.FirstOrDefault(x => x.Name == csvProduct.Name && !x.IsTransient());
				if (sameProduct != null)
				{
					//Detect variation
					csvProduct.MainProductId = sameProduct.Id;
				}
				var category = newCategories.FirstOrDefault(x => x.Code == csvProduct.Category.Code);
				csvProduct.CategoryId = category.Id;
				csvProduct.CatalogId = catalog.Id;

				try
				{
					if(String.IsNullOrEmpty(csvProduct.Code))
					{
						csvProduct.Code = _skuGenerator.GenerateSku(csvProduct);
					}
					var newProduct = _productService.Create(csvProduct);
					csvProduct.Id = newProduct.Id;
				}
				catch (Exception ex)
				{
					notification.ErrorCount++;
					notification.Errors.Add(ex.ToString());
					_notifier.Upsert(notification);
				}
				finally
				{
					//Raise notification each notifyProductSizeLimit category
					notification.ProcessedCount = counter;
					notification.Description = string.Format("Creating products: {0} of {1} created", notification.ProcessedCount, notification.TotalCount);
					counter++;
					if (counter % notifyProductSizeLimit == 0)
					{
						_notifier.Upsert(notification);
					}
				}
			}

			notification.Description = "Import finished" + (notification.Errors.Any() ? " with errors" : " successfully");
			_notifier.Upsert(notification);
		}

		private void SaveCategoryRecursive(List<coreModel.Category> categories, coreModel.Category category, ImportNotification notification)
		{
			//save to db
			//Categories
			var newCategory = _categoryService.Create(category);
			categories.Add(newCategory);
			//TODO: Prevent category creation if it exist
			foreach (var childCategory in category.Children)
			{
				childCategory.ParentId = newCategory.Id;
				SaveCategoryRecursive(categories, childCategory, notification);
			
				//Raise notification each notifyCategorySizeLimit category
				notification.Description = string.Format("Creating categories: {0} of {1} created", notification.ProcessedCount, notification.TotalCount);
				_notifier.Upsert(notification);
			}
		}


		public sealed class ProductMap : CsvClassMap<coreModel.CatalogProduct>
		{
			public ProductMap(string[] allColumns, webModel.CsvImportConfiguration importConfiguration, coreModel.Catalog catalog)
			{

				var defaultLanguge = catalog.DefaultLanguage != null ? catalog.DefaultLanguage.LanguageCode : "EN-US";
				//Dynamical map scalar product fields use by manual mapping information
				foreach (var mappingConfigurationItem in importConfiguration.MappingItems.Where(x => x.CsvColumnName != null || x.CustomValue != null))
				{
					var propertyInfo = typeof(coreModel.CatalogProduct).GetProperty(mappingConfigurationItem.EntityColumnName);
					if (propertyInfo != null)
					{
						var newMap = new CsvPropertyMap(propertyInfo);
						//Map fields if mapping specified
						if (mappingConfigurationItem.CsvColumnName != null)
						{
							newMap.Name(mappingConfigurationItem.CsvColumnName);
						}
						//And default values if it specified
						if (mappingConfigurationItem.CustomValue != null)
						{
							newMap.Default(mappingConfigurationItem.CustomValue);
						}
						PropertyMaps.Add(newMap);
					}
				}

				//Map assets (images)
				Map(x => x.Assets).ConvertUsing(x =>
					{
						var retVal = new List<coreModel.ItemAsset>();
						var primaryImageUrl = GetCsvField("PrimaryImage", x, importConfiguration);
						var altImageUrl = GetCsvField("AltImage", x, importConfiguration);
						if (!String.IsNullOrEmpty(primaryImageUrl))
						{
							retVal.Add(new coreModel.ItemAsset
							{
								Type = coreModel.ItemAssetType.Image,
								Group = "primaryimage",
								Url = primaryImageUrl
							});
						}

						if (!String.IsNullOrEmpty(altImageUrl))
						{
							retVal.Add(new coreModel.ItemAsset
							{
								Type = coreModel.ItemAssetType.Image,
								Group = "image",
								Url = altImageUrl
							});
						}

						return retVal;
					});

				//Map category
				Map(x => x.Category).ConvertUsing(x => new coreModel.Category { Path = GetCsvField("Category", x, importConfiguration) });

				//Map Reviews
				Map(x => x.Reviews).ConvertUsing(x =>
				{
					var reviews = new List<coreModel.EditorialReview>();
					var content = GetCsvField("Review", x, importConfiguration);
					if (!String.IsNullOrEmpty(content))
					{
						reviews.Add(new coreModel.EditorialReview { LanguageCode = defaultLanguge });
					}
					return reviews;

				});

				//Map Seo
				Map(x => x.SeoInfos).ConvertUsing(x =>
				{
					var seoInfos = new List<coreModel.SeoInfo>();
					var seoUrl = GetCsvField("SeoUrl", x, importConfiguration);
					var seoDescription = GetCsvField("SeoDescription", x, importConfiguration);
					var seoTitle = GetCsvField("SeoTitle", x, importConfiguration);
					if (!String.IsNullOrEmpty(seoUrl) || !String.IsNullOrEmpty(seoTitle) || !String.IsNullOrEmpty(seoDescription))
					{
						seoUrl = new string[] { seoUrl, seoDescription, seoTitle }.Where(y => !String.IsNullOrEmpty(y)).FirstOrDefault();
						seoUrl = seoUrl.Substring(0, Math.Min(seoUrl.Length, 240));
						seoInfos.Add(new coreModel.SeoInfo { LanguageCode = defaultLanguge, SemanticUrl = seoUrl.GenerateSlug(), MetaDescription = seoDescription, PageTitle = seoTitle});
					}
					return seoInfos; 
				});

				//Map properties
				if (importConfiguration.PropertyCsvColumns != null)
				{
					Map(x => x.PropertyValues).ConvertUsing(x => importConfiguration.PropertyCsvColumns.Select(column => new coreModel.PropertyValue { PropertyName = column, Value = x.GetField<string>(column) }).ToList());
				}
			}

			private static string GetCsvField(string name, ICsvReaderRow row, webModel.CsvImportConfiguration configuration)
			{
				var mapping = configuration.MappingItems.First(y => y.EntityColumnName == name);
				if (mapping == null)
				{
					throw new NullReferenceException("mapping");
				}

				var retVal = mapping.CustomValue;
				if (mapping.CsvColumnName != null)
				{
					retVal = row.GetField<string>(mapping.CsvColumnName);
				}
				return retVal;
			}
		}



	}
}
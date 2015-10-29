using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Common;
using System.IO;
using CsvHelper;
using System.Text;
using System.Reflection;
using VirtoCommerce.CatalogModule.Web.Model.EventNotifications;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using CsvHelper.Configuration;
using VirtoCommerce.Domain.Pricing.Model;
using System.Globalization;
using VirtoCommerce.Domain.Inventory.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Commerce.Services;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.PushNotifications;


namespace VirtoCommerce.CatalogModule.Web.ExportImport
{
	public sealed class CsvCatalogImporter
	{
		private readonly char[] _categoryDelimiters = new char[] { '/', '|', '\\', '>' };
		private readonly ICatalogService _catalogService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _productService;
		private readonly IPushNotificationManager _pushNotificationManager;
		private readonly CacheManager _cacheManager;
		private readonly ISkuGenerator _skuGenerator;
		private readonly IPricingService _pricingService;
		private readonly IInventoryService _inventoryService;
		private readonly ICommerceService _commerceService;
		private readonly IPropertyService _propertyService;
		private readonly ICatalogSearchService _searchService;
		private object _lockObject = new object();

		public CsvCatalogImporter(ICatalogService catalogService, ICategoryService categoryService, IItemService productService,
								IPushNotificationManager pushNotificationManager, CacheManager cacheManager, ISkuGenerator skuGenerator,
								IPricingService pricingService, IInventoryService inventoryService, ICommerceService commerceService,
								IPropertyService propertyService, ICatalogSearchService searchService)
		{
			_catalogService = catalogService;
			_categoryService = categoryService;
			_productService = productService;
			_pushNotificationManager = pushNotificationManager;
			_cacheManager = cacheManager;
			_skuGenerator = skuGenerator;
			_pricingService = pricingService;
			_inventoryService = inventoryService;
			_commerceService = commerceService;
			_propertyService = propertyService;
			_searchService = searchService;
		}

		public void DoImport(Stream inputStream, CsvImportInfo importInfo, Action<ExportImportProgressInfo> progressCallback)
		{
			var csvProducts = new List<CsvProduct>();


			var progressInfo = new ExportImportProgressInfo
			{
				Description = "Reading products from csv..."
			};
			progressCallback(progressInfo);


			using (var reader = new CsvReader(new StreamReader(inputStream)))
			{
				reader.Configuration.Delimiter = importInfo.Configuration.Delimiter;
				reader.Configuration.RegisterClassMap(new CsvProductMap(importInfo.Configuration));
                reader.Configuration.WillThrowOnMissingField = false;

				while (reader.Read())
				{
					try
					{
						var csvProduct = reader.GetRecord<CsvProduct>();
						csvProducts.Add(csvProduct);
					}
					catch (Exception ex)
					{
						var error = ex.Message;
						if (ex.Data.Contains("CsvHelper"))
						{
							error += ex.Data["CsvHelper"];
						}
						progressInfo.Errors.Add(error);
						progressCallback(progressInfo);
					}
				}
			}


			var catalog = _catalogService.GetById(importInfo.CatalogId);

			SaveCategoryTree(catalog, csvProducts, progressInfo, progressCallback);
			SaveProducts(catalog, csvProducts, progressInfo, progressCallback);
		}

		private void SaveCategoryTree(coreModel.Catalog catalog, IEnumerable<CsvProduct> csvProducts, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback)
		{
			var categories = new List<coreModel.Category>();
			progressInfo.ProcessedCount = 0;
			var cachedCategoryMap = new Dictionary<string, Category>();

			foreach (var csvProduct in csvProducts.Where(x => x.Category != null && !String.IsNullOrEmpty(x.Category.Path)))
			{
				var outline = "";
				var productCategoryNames = csvProduct.Category.Path.Split(_categoryDelimiters);
				string parentCategoryId = null;
				foreach (var categoryName in productCategoryNames)
				{
					outline += "\\" + categoryName;
					Category category;
					if (!cachedCategoryMap.TryGetValue(outline, out category))
					{
						var searchCriteria = new SearchCriteria
						{
							CatalogId = catalog.Id,
							CategoryId = parentCategoryId,
							ResponseGroup = ResponseGroup.WithCategories
						};
						category = _searchService.Search(searchCriteria).Categories.FirstOrDefault(x => x.Name == categoryName);
					}

					if (category == null)
					{
						category = _categoryService.Create(new coreModel.Category() { Name = categoryName, Code = categoryName.GenerateSlug(), CatalogId = catalog.Id, ParentId = parentCategoryId });
						//Raise notification each notifyCategorySizeLimit category
						progressInfo.Description = string.Format("Creating categories: {0} created", ++progressInfo.ProcessedCount);
						progressCallback(progressInfo);
					}
					csvProduct.CategoryId = category.Id;
					csvProduct.Category = category;
					parentCategoryId = category.Id;
					cachedCategoryMap[outline] = category;
				}
			}

		}


		private void SaveProducts(coreModel.Catalog catalog, IEnumerable<CsvProduct> csvProducts, ExportImportProgressInfo progressInfo, Action<ExportImportProgressInfo> progressCallback)
		{
			int counter = 0;
			var notifyProductSizeLimit = 10;
			progressInfo.TotalCount = csvProducts.Count();

			var defaultFulfilmentCenter = _commerceService.GetAllFulfillmentCenters().FirstOrDefault();
			var options = new ParallelOptions()
			{
				MaxDegreeOfParallelism = 10
			};

			DetectParents(csvProducts);

			//First need create main products, second will be created variations
			foreach (var group in new IEnumerable<CsvProduct>[] { csvProducts.Where(x => x.MainProduct == null), csvProducts.Where(x => x.MainProduct != null) })
			{
				Parallel.ForEach(group, options, csvProduct =>
				{
					try
					{
						SaveProduct(catalog, defaultFulfilmentCenter, csvProduct);
					}
					catch (Exception ex)
					{
						lock (_lockObject)
						{
							progressInfo.Errors.Add(ex.ToString());
							progressCallback(progressInfo);
						}
					}
					finally
					{
						lock (_lockObject)
						{
							//Raise notification each notifyProductSizeLimit category
							counter++;
							progressInfo.ProcessedCount = counter;
							progressInfo.Description = string.Format("Creating products: {0} of {1} created", progressInfo.ProcessedCount, progressInfo.TotalCount);
							if (counter % notifyProductSizeLimit == 0 || counter == progressInfo.TotalCount)
							{
								progressCallback(progressInfo);
							}
						}
					}
				});
			};
		}

		private void DetectParents(IEnumerable<CsvProduct> csvProducts)
		{
			foreach (var csvProduct in csvProducts)
			{
				//Try to set parent relations
				//By id or code reference
				var parentProduct = csvProducts.FirstOrDefault(x => csvProduct.MainProductId != null && (x.Id == csvProduct.MainProductId || x.Code == csvProduct.MainProductId));
				csvProduct.MainProduct = parentProduct;
				csvProduct.MainProductId = parentProduct != null ? parentProduct.Id : null;
			}
		}

		private void SaveProduct(coreModel.Catalog catalog, FulfillmentCenter defaultFulfillmentCenter, CsvProduct csvProduct)
		{
			var defaultLanguge = catalog.DefaultLanguage != null ? catalog.DefaultLanguage.LanguageCode : "EN-US";

			coreModel.CatalogProduct alreadyExistProduct = null;
			//For new product try to find them by code
			if (csvProduct.IsTransient() && !String.IsNullOrEmpty(csvProduct.Code))
			{
				var criteria = new SearchCriteria
				{
					CatalogId = catalog.Id,
					CategoryId = csvProduct.CategoryId,
					Code = csvProduct.Code,
					ResponseGroup = ResponseGroup.WithProducts | ResponseGroup.WithVariations
				};
				var result = _searchService.Search(criteria);
				alreadyExistProduct = result.Products.FirstOrDefault();
				csvProduct.Id = alreadyExistProduct != null ? alreadyExistProduct.Id : csvProduct.Id;
			}
			else if (!csvProduct.IsTransient())
			{
				//If id specified need check that product really exist 
				alreadyExistProduct = _productService.GetById(csvProduct.Id, ItemResponseGroup.ItemInfo);
			}
			var isNewProduct = alreadyExistProduct == null;

			csvProduct.CatalogId = catalog.Id;

			if (String.IsNullOrEmpty(csvProduct.Code))
			{
				csvProduct.Code = _skuGenerator.GenerateSku(csvProduct);
			}
			//Set a parent relations
			if (csvProduct.MainProductId == null && csvProduct.MainProduct != null)
			{
				csvProduct.MainProductId = csvProduct.MainProduct.Id;
			}
			csvProduct.EditorialReview.LanguageCode = defaultLanguge;
			csvProduct.SeoInfo.LanguageCode = defaultLanguge;
			csvProduct.SeoInfo.SemanticUrl = String.IsNullOrEmpty(csvProduct.SeoInfo.SemanticUrl) ? csvProduct.Code : csvProduct.SeoInfo.SemanticUrl;

			var properties = !String.IsNullOrEmpty(csvProduct.CategoryId) ? _propertyService.GetCategoryProperties(csvProduct.CategoryId) : _propertyService.GetCatalogProperties(csvProduct.CatalogId);

			if (csvProduct.PropertyValues != null)
			{
				//Try to fill properties meta information for values
				foreach (var propertyValue in csvProduct.PropertyValues)
				{
					if (propertyValue.Value != null)
					{
						var property = properties.FirstOrDefault(x => String.Equals(x.Name, propertyValue.PropertyName));
						if (property != null)
						{
							propertyValue.ValueType = property.ValueType;
							if (property.Dictionary)
							{
								property = _propertyService.GetById(property.Id);
								var dicValue = property.DictionaryValues.FirstOrDefault(x => String.Equals(x.Value, propertyValue.Value));
								propertyValue.ValueId = dicValue != null ? dicValue.Id : null;
							}
						}
					}
				}
			}

			if (!isNewProduct)
			{
				_productService.Update(new coreModel.CatalogProduct[] { csvProduct });
			}
			else
			{
				var newProduct = _productService.Create(csvProduct);
				csvProduct.Id = newProduct.Id;
			}

			//Create price in default price list

			if (csvProduct.Price.EffectiveValue > 0)
			{
				csvProduct.Price.ProductId = csvProduct.Id;

				if (csvProduct.Price.IsTransient() || _pricingService.GetPriceById(csvProduct.Price.Id) == null)
				{
					_pricingService.CreatePrice(csvProduct.Price);
				}
				else
				{
					_pricingService.UpdatePrices(new Price[] { csvProduct.Price });
				}
			}

			//Create inventory
			csvProduct.Inventory.ProductId = csvProduct.Id;
			csvProduct.Inventory.FulfillmentCenterId = csvProduct.Inventory.FulfillmentCenterId ?? defaultFulfillmentCenter.Id;
			_inventoryService.UpsertInventory(csvProduct.Inventory);
		}

	}
}
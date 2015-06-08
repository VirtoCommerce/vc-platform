using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Catalog.Model;
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
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Inventory.Model;
using System.Globalization;

namespace VirtoCommerce.CatalogModule.Web.BackgroundJobs
{
	public class CsvCatalogExportJob
	{
		private readonly ICatalogSearchService _searchService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _productService;
		private readonly INotifier _notifier;
		private readonly CacheManager _cacheManager;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly IBlobUrlResolver _blobUrlResolver;
		private readonly IPricingService _pricingService;
		private readonly IInventoryService _inventoryService;

		public CsvCatalogExportJob(ICatalogSearchService catalogSearchService,
								ICategoryService categoryService, IItemService productService,
								INotifier notifier, CacheManager cacheManager, IBlobStorageProvider blobProvider, IBlobUrlResolver blobUrlResolver,
								IPricingService pricingService, IInventoryService inventoryService)
		{
			_searchService = catalogSearchService;
			_categoryService = categoryService;
			_productService = productService;
			_notifier = notifier;
			_cacheManager = cacheManager;
			_blobStorageProvider = blobProvider;
			_blobUrlResolver = blobUrlResolver;
			_pricingService = pricingService;
			_inventoryService = inventoryService;
		}

		public virtual void DoExport(string catalogId, string[] exportedCategories, string[] exportedProducts, CurrencyCodes currency, string languageCode, ExportNotification notification)
		{
			var memoryStream = new MemoryStream();
			var streamWriter = new StreamWriter(memoryStream);
			streamWriter.AutoFlush = true;
			var productPropertyInfos = typeof(CatalogProduct).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			string catalogName = null;
			using (var csvWriter = new CsvWriter(streamWriter))
			{
				csvWriter.Configuration.Delimiter = ";";

				//Notification
				notification.Description = "loading products...";
				_notifier.Upsert(notification);

				try
				{
					//Load all products to export
					var products = LoadProducts(catalogId, exportedCategories, exportedProducts);

					//Notification
					notification.Description = "loading prices...";
					_notifier.Upsert(notification);
					var allProductIds = products.Select(x=>x.Id).ToArray();
					//Load prices for products
					var priceEvalContext = new  PriceEvaluationContext {
						ProductIds = allProductIds,
						Currency = currency
					};
					var allProductPrices = _pricingService.EvaluateProductPrices(priceEvalContext).ToArray();
					foreach(var product in products)
					{
						product.Prices = allProductPrices.Where(x => x.ProductId == product.Id).ToList();
					}
					//Load inventories
					notification.Description = "loading inventory information...";
					_notifier.Upsert(notification);
					var allProductInventories = _inventoryService.GetProductsInventoryInfos(allProductIds);
					foreach (var product in products)
					{
						product.Inventories = allProductInventories.Where(x => x.ProductId == product.Id).ToList();
					}

					notification.TotalCount = products.Count();
					//populate export configuration
					Dictionary<string, Func<CatalogProduct, string>> exportConfiguration = new Dictionary<string, Func<CatalogProduct, string>>();
					PopulateProductExportConfiguration(exportConfiguration, products);

					//Write header
					foreach (var cfgItem in exportConfiguration)
					{
						csvWriter.WriteField(cfgItem.Key);
					}
					csvWriter.NextRecord();

					var notifyProductSizeLimit = 50;
					var counter = 0;
					//Write products
					foreach (var product in products)
					{
						if(catalogName == null && product.Catalog != null)
						{
							catalogName = product.Catalog.Name;
						}

						try
						{
							foreach (var cfgItem in exportConfiguration)
							{
								var fieldValue = String.Empty;
								if (cfgItem.Value == null)
								{
									var propertyInfo = productPropertyInfos.FirstOrDefault(x => x.Name == cfgItem.Key);
									if (propertyInfo != null)
									{
										var objValue = propertyInfo.GetValue(product);
										fieldValue = objValue != null ? objValue.ToString() : fieldValue;
									}
								}
								else
								{
									fieldValue = cfgItem.Value(product);
								}
								csvWriter.WriteField(fieldValue);
							}
							csvWriter.NextRecord();
						}
						catch(Exception ex)
						{
							notification.ErrorCount++;
							notification.Errors.Add(ex.ToString());
							_notifier.Upsert(notification);
						}

						//Raise notification each notifyProductSizeLimit products
						counter++;
						notification.ProcessedCount = counter;
						notification.Description = string.Format("{0} of {1} products processed", notification.ProcessedCount, notification.TotalCount);
						if (counter % notifyProductSizeLimit == 0)
						{
							_notifier.Upsert(notification);
						}
					}
					memoryStream.Position = 0;
					//Upload result csv to blob storage
					var uploadInfo = new UploadStreamInfo
					{
						FileName = "Catalog-" + (catalogName ?? catalogId) + "-export.csv",
						FileByteStream = memoryStream,
						FolderName = "export"
					};
					var blobKey = _blobStorageProvider.Upload(uploadInfo);
					//Get a download url
					notification.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(blobKey);
					notification.Description = "Export finished";
				}
				catch(Exception ex)
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
			
		}

		private IEnumerable<CatalogProduct> LoadProducts(string catalogId, string[] exportedCategories, string[] exportedProducts)
		{
			var retVal = new List<CatalogProduct>();
		
				var productIds = new List<string>();
				if(exportedProducts != null)
				{
					productIds = exportedProducts.ToList();
				}
				if (exportedCategories != null && exportedCategories.Any())
				{
					foreach (var categoryId in exportedCategories)
					{
						var result = _searchService.Search(new SearchCriteria { CatalogId = catalogId, CategoryId = categoryId, Start = 0, Count = int.MaxValue, ResponseGroup = ResponseGroup.WithProducts | ResponseGroup.WithCategories });
						productIds.AddRange(result.Products.Select(x => x.Id));
						if (result.Categories != null && result.Categories.Any())
						{
							retVal.AddRange(LoadProducts(catalogId, result.Categories.Select(x => x.Id).ToArray(), null));
						}
					}
				}

				if ((exportedCategories == null || !exportedCategories.Any()) && (exportedProducts == null || !exportedProducts.Any()))
				{
					var result = _searchService.Search(new SearchCriteria { CatalogId = catalogId, GetAllCategories = true, Start = 0, Count = int.MaxValue, ResponseGroup = ResponseGroup.WithProducts });
					productIds = result.Products.Select(x => x.Id).ToList();
				}

				var products = _productService.GetByIds(productIds.Distinct().ToArray(), ItemResponseGroup.ItemLarge);
				foreach(var product in products)
				{
					retVal.Add(product);
					if (product.Variations != null)
					{
						retVal.AddRange(product.Variations);
					}
				}

			return retVal;
		}

		
		private void PopulateProductExportConfiguration(Dictionary<string, Func<CatalogProduct, string>> configuration, IEnumerable<CatalogProduct> products)
		{

			configuration.Add("Sku", (product) =>
			{
				return product.Code;
			});

			configuration.Add("ParentSku", (product) =>
			{
				return product.MainProduct != null ? product.MainProduct.Code : String.Empty;
			});

			//Category
			configuration.Add("Category", (product) =>
			{
				if(product.CategoryId != null)
				{
					var category = _categoryService.GetById(product.CategoryId);
					var path = String.Join("/", category.Parents.Select(x => x.Name).Concat(new string[] { category.Name }));
					return path;
				}
				return String.Empty;
			});


			//Scalar Properties
			var exportScalarFields = ReflectionUtility.GetPropertyNames<CatalogProduct>(x=>x.Id, x=>x.MainProductId, x=>x.CategoryId, x => x.Name, x => x.IsActive, x => x.IsBuyable, x => x.TrackInventory,
																							  x => x.ManufacturerPartNumber, x => x.Gtin, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight,
																							  x => x.Height, x => x.Length, x => x.Width, x => x.TaxType,  x => x.ShippingType,
																							  x => x.ProductType, x => x.Vendor, x => x.DownloadType, x => x.DownloadExpiration, x => x.HasUserAgreement);
			configuration.AddRange(exportScalarFields.Select(x => new KeyValuePair<string, Func<CatalogProduct, string>>(x, null)));

			configuration.Add("PrimaryImage", (product) =>
			{
				if (product.Assets != null)
				{
					var image = product.Assets.Where(x => x.Type == ItemAssetType.Image && x.Group == "primaryimage").FirstOrDefault();
					if (image != null)
					{
						return _blobUrlResolver.GetAbsoluteUrl(image.Url);
					}
				}
				return String.Empty;
			});

			configuration.Add("AltImage", (product) =>
			{
				if (product.Assets != null)
				{
					var image = product.Assets.Where(x => x.Type == ItemAssetType.Image && x.Group != "primaryimage").FirstOrDefault();
					if (image != null)
					{
						return _blobUrlResolver.GetAbsoluteUrl(image.Url);
					}
				}
				return String.Empty;
			});

			configuration.Add("Review", (product) =>
			{
				if (product.Reviews != null)
				{
					var review = product.Reviews.FirstOrDefault();
					if (review != null)
					{
						return review.Content ?? String.Empty;
					}
				}
				return String.Empty;
			});

			configuration.Add("SeoUrl", (product) =>
			{
				if (product.SeoInfos != null)
				{
					var seoInfo = product.SeoInfos.FirstOrDefault();
					if (seoInfo != null)
					{
						return seoInfo.SemanticUrl ?? String.Empty;
					}
				}
				return String.Empty;
			});
			configuration.Add("SeoTitle", (product) =>
			{
				if (product.SeoInfos != null)
				{
					var seoInfo = product.SeoInfos.FirstOrDefault();
					if (seoInfo != null)
					{
						return seoInfo.PageTitle ?? String.Empty;
					}
				}
				return String.Empty;
			});
			configuration.Add("SeoDescription", (product) =>
			{
				if (product.SeoInfos != null)
				{
					var seoInfo = product.SeoInfos.FirstOrDefault();
					if (seoInfo != null)
					{
						return seoInfo.MetaDescription ?? String.Empty;
					}
				}
				return String.Empty;
			});

			//Prices
			configuration.Add("PriceId", (product) =>
			{
				var bestPrice = product.Prices.Any() ? product.Prices.FirstOrDefault() : null;
				return bestPrice != null ? bestPrice.Id : String.Empty;
			});
			configuration.Add("SalePrice", (product) =>
				{
					var bestPrice = product.Prices.Any() ? product.Prices.FirstOrDefault() : null;
					return bestPrice != null ? (bestPrice.Sale != null ? bestPrice.Sale.Value.ToString(CultureInfo.InvariantCulture) : String.Empty) : String.Empty;
				});
			configuration.Add("Price", (product) =>
			{
				var bestPrice = product.Prices.Any() ? product.Prices.FirstOrDefault() : null;
				return bestPrice != null ? bestPrice.List.ToString(CultureInfo.InvariantCulture) : String.Empty;
			});
			configuration.Add("Currency", (product) =>
			{
				var bestPrice = product.Prices.Any() ? product.Prices.FirstOrDefault() : null;
				return bestPrice != null ? bestPrice.Currency.ToString() : String.Empty;
			});
		
			//Inventories
			configuration.Add("AllowBackorder", (product) =>
			{
				var inventory = product.Inventories.Any() ? product.Inventories.FirstOrDefault() : null;
				return inventory != null ? inventory.AllowBackorder.ToString() : String.Empty;
			});
			configuration.Add("Quantity", (product) =>
			{
				var inventory = product.Inventories.Any() ? product.Inventories.FirstOrDefault() : null;
				return inventory != null ? inventory.InStockQuantity.ToString() : String.Empty;
			});
		
			//Properties
			foreach (var propertyName in products.SelectMany(x => x.PropertyValues).Select(x => x.PropertyName).Distinct())
			{
				if (!configuration.ContainsKey(propertyName))
				{
					configuration.Add(propertyName, (product) =>
					{
						var propertyValue = product.PropertyValues.FirstOrDefault(x => x.PropertyName == propertyName);
						if (propertyValue != null)
						{
							return propertyValue.Value != null ? propertyValue.Value.ToString() : String.Empty;
						}
						return String.Empty;
					});
				}
			}
	
		}

	}
}
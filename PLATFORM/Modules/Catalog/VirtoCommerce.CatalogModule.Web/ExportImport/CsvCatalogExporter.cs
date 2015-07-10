using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.CatalogModule.Web.ExportImport
{
	public sealed class CsvCatalogExporter
	{
		private readonly ICatalogSearchService _searchService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _productService;
		private readonly IPricingService _pricingService;
		private readonly IInventoryService _inventoryService;
		private readonly IBlobUrlResolver _blobUrlResolver;

		public CsvCatalogExporter(ICatalogSearchService catalogSearchService,
								  ICategoryService categoryService, IItemService productService,
								  IPricingService pricingService, IInventoryService inventoryService, IBlobUrlResolver blobUrlResolver)
		{
			_searchService = catalogSearchService;
			_categoryService = categoryService;
			_productService = productService;
			_pricingService = pricingService;
			_inventoryService = inventoryService;
			_blobUrlResolver = blobUrlResolver;
		}

		public void DoExport(Stream outStream, string catalogId, string[] exportedCategories, string[] exportedProducts, string pricelistId, string fulfilmentCenterId, CurrencyCodes currency, string languageCode, Action<ExportImportProgressInfo> progressCallback)
		{
			var prodgressInfo = new ExportImportProgressInfo
			{
				Description = "loading products..."
			};

			var streamWriter = new StreamWriter(outStream, Encoding.UTF8, 1024, true);
			streamWriter.AutoFlush = true;
			var productPropertyInfos = typeof(CatalogProduct).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			using (var csvWriter = new CsvWriter(streamWriter))
			{
				csvWriter.Configuration.Delimiter = ";";

				//Notification
				progressCallback(prodgressInfo);


				//Load all products to export
				var products = LoadProducts(catalogId, exportedCategories, exportedProducts).ToArray();

				//Notification
				prodgressInfo.Description = "loading prices...";
				progressCallback(prodgressInfo);

				var allProductIds = products.Select(x => x.Id).ToArray();
				//Load prices for products
				var priceEvalContext = new PriceEvaluationContext
				{
					ProductIds = allProductIds,
					PricelistIds = pricelistId == null ? null : new string[] { pricelistId },
					Currency = currency
				};

				var allProductPrices = _pricingService.EvaluateProductPrices(priceEvalContext).ToArray();
				foreach (var product in products)
				{
					product.Prices = allProductPrices.Where(x => x.ProductId == product.Id).ToList();
				}
				//Load inventories
				prodgressInfo.Description = "loading inventory information...";
				progressCallback(prodgressInfo);

				var allProductInventories = _inventoryService.GetProductsInventoryInfos(allProductIds).ToArray();
				foreach (var product in products)
				{
					product.Inventories = allProductInventories.Where(x => x.ProductId == product.Id)
						.Where(x => fulfilmentCenterId == null || x.FulfillmentCenterId == fulfilmentCenterId).ToList();
				}

				prodgressInfo.TotalCount = products.Count();
				//populate export configuration
				var exportConfiguration = new Dictionary<string, Func<CatalogProduct, string>>();
				PopulateProductExportConfiguration(exportConfiguration, products, languageCode);

				//Write header
				foreach (var cfgItem in exportConfiguration)
				{
					csvWriter.WriteField(cfgItem.Key);
				}
				csvWriter.NextRecord();

				const int notifyProductSizeLimit = 50;
				var counter = 0;
				//Write products
				foreach (var product in products)
				{
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
					catch (Exception ex)
					{
						prodgressInfo.ErrorCount++;
						prodgressInfo.Errors.Add(ex.ToString());
						progressCallback(prodgressInfo);
					}

					//Raise notification each notifyProductSizeLimit products
					counter++;
					prodgressInfo.ProcessedCount = counter;
					prodgressInfo.Description = string.Format("{0} of {1} products processed", prodgressInfo.ProcessedCount, prodgressInfo.TotalCount);
					if (counter % notifyProductSizeLimit == 0)
					{
						progressCallback(prodgressInfo);
					}
				}
			}

		}


		private IEnumerable<CatalogProduct> LoadProducts(string catalogId, string[] exportedCategories, string[] exportedProducts)
		{
			var retVal = new List<CatalogProduct>();

			var productIds = new List<string>();
			if (exportedProducts != null)
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
			foreach (var product in products)
			{
				retVal.Add(product);
				if (product.Variations != null)
				{
					retVal.AddRange(product.Variations);
				}
			}

			return retVal;
		}


		private void PopulateProductExportConfiguration(Dictionary<string, Func<CatalogProduct, string>> configuration, IEnumerable<CatalogProduct> products, string languageCode)
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
				if (product.CategoryId != null)
				{
					var category = _categoryService.GetById(product.CategoryId);
					var path = String.Join("/", category.Parents.Select(x => x.Name).Concat(new string[] { category.Name }));
					return path;
				}
				return String.Empty;
			});


			//Scalar Properties
			var exportScalarFields = ReflectionUtility.GetPropertyNames<CatalogProduct>(x => x.Id, x => x.MainProductId, x => x.CategoryId, x => x.Name, x => x.IsActive, x => x.IsBuyable, x => x.TrackInventory,
																							  x => x.ManufacturerPartNumber, x => x.Gtin, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight,
																							  x => x.Height, x => x.Length, x => x.Width, x => x.TaxType, x => x.ShippingType,
																							  x => x.ProductType, x => x.Vendor, x => x.DownloadType, x => x.DownloadExpiration, x => x.HasUserAgreement);
			configuration.AddRange(exportScalarFields.Select(x => new KeyValuePair<string, Func<CatalogProduct, string>>(x, null)));

			configuration.Add("PrimaryImage", (product) =>
			{
				if (product.Images != null && product.Images.Any())
				{
					return _blobUrlResolver.GetAbsoluteUrl(product.Images.First().Url);
				}
				return String.Empty;
			});

			configuration.Add("AltImage", (product) =>
			{
				if (product.Images != null)
				{
                    var image = product.Images.Skip(1).FirstOrDefault(x => x.LanguageCode == languageCode);
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
                    var review = product.Reviews.FirstOrDefault(x => x.LanguageCode == languageCode);
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
                    var seoInfo = product.SeoInfos.FirstOrDefault(x => x.LanguageCode == languageCode);
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
                    var seoInfo = product.SeoInfos.FirstOrDefault(x => x.LanguageCode == languageCode);
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
                    var seoInfo = product.SeoInfos.FirstOrDefault(x => x.LanguageCode == languageCode);
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
			configuration.Add("FulfilmentCenterId", (product) =>
			{
				var inventory = product.Inventories.Any() ? product.Inventories.FirstOrDefault() : null;
				return inventory != null ? inventory.FulfillmentCenterId : String.Empty;
			});
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
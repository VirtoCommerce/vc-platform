using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using VirtoCommerce.CatalogModule.Web.BackgroundJobs;
using VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.CatalogModule.Web.Model.Notifications;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;
using System.Linq;
using CsvHelper;
using System.IO;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using System.Collections.Generic;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Commerce.Services;
namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog")]
    public class CatalogModuleExportImportController : ApiController
    {
		private readonly ICatalogService _catalogService;
		private readonly ICatalogSearchService _searchService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _productService;
		private readonly INotifier _notifier;
		private readonly CacheManager _cacheManager;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly IBlobUrlResolver _blobUrlResolver;
		private readonly ISkuGenerator _skuGenerator;
		private readonly IPricingService _pricingService;
		private readonly IInventoryService _inventoryService;
		private readonly ICommerceService _commerceService;

		public CatalogModuleExportImportController(ICatalogService catalogService, ICatalogSearchService catalogSearchService,
								ICategoryService categoryService, IItemService productService,
								INotifier notifier, CacheManager cacheManager, IBlobStorageProvider blobProvider,
								IBlobUrlResolver blobUrlResolver, ISkuGenerator skuGenerator, IPricingService pricingService,
								IInventoryService inventoryService, ICommerceService commerceService) 
		{
			_catalogService = catalogService;
			_searchService = catalogSearchService;
			_categoryService = categoryService;
			_productService = productService;
			_notifier = notifier;
			_cacheManager = cacheManager;
			_blobStorageProvider = blobProvider;
			_blobUrlResolver = blobUrlResolver;
			_skuGenerator = skuGenerator;
			_pricingService = pricingService;
			_inventoryService = inventoryService;
			_commerceService = commerceService;

		}

		/// <summary>
		/// GET api/catalog/export/sony
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[ResponseType(typeof(ExportNotification))]
		[HttpPost]
		[Route("export")]
		public IHttpActionResult DoExport(CsvExportInfo exportInfo)
		{
			var notification = new ExportNotification(CurrentPrincipal.GetCurrentUserName())
			{
				Title = "Catalog export task",
				Description = "starting export...."
			};
			_notifier.Upsert(notification);

			var catalog = _catalogService.GetById(exportInfo.CatalogId);
			if(catalog == null)
			{
				throw new NullReferenceException("catalog");
			}
			var exportJob = new CsvCatalogExportJob(_searchService, _categoryService, _productService, _notifier, _cacheManager, _blobStorageProvider, _blobUrlResolver, _pricingService, _inventoryService);
			BackgroundJob.Enqueue(() => exportJob.DoExport(exportInfo.CatalogId, exportInfo.CategoryIds, exportInfo.ProductIds, exportInfo.Currency ?? CurrencyCodes.USD, catalog.DefaultLanguage.LanguageCode, notification));

			return Ok(notification);

		}


		/// <summary>
		/// GET api/catalog/import/mapping?path='c:\\sss.csv'&importType=product&delimiter=,
		/// </summary>
		/// <param name="templatePath"></param>
		/// <param name="importerType"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		[ResponseType(typeof(CsvImportConfiguration))]
		[HttpGet]
		[Route("import/mappingconfiguration")]
		public IHttpActionResult GetMappingConfiguration([FromUri]string fileUrl, [FromUri]string delimiter = ";")
		{
			var retVal = new CsvImportConfiguration
				{
					Delimiter = delimiter,
					FileUrl = fileUrl
				};
			var mappingItems = new List<CsvImportMappingItem>();

			mappingItems.AddRange(ReflectionUtility.GetPropertyNames<coreModel.CatalogProduct>(x => x.Name, x => x.Category).Select(x => new CsvImportMappingItem { EntityColumnName = x, IsRequired = true }));

			mappingItems.AddRange(new string[] {"Sku", "ParentSku", "Review", "PrimaryImage", "AltImage", "SeoUrl", "SeoDescription", "SeoTitle", 
												"PriceId", "Price", "SalePrice", "Currency", "AllowBackorder", "Quantity" }
								   .Select(x => new CsvImportMappingItem { EntityColumnName = x, IsRequired = false }));

			mappingItems.AddRange(ReflectionUtility.GetPropertyNames<coreModel.CatalogProduct>(x=>x.Id, x=>x.MainProductId, x=>x.CategoryId, x => x.IsActive, x => x.IsBuyable, x => x.TrackInventory,
																							  x => x.ManufacturerPartNumber, x => x.Gtin, x => x.MeasureUnit, x => x.WeightUnit, x => x.Weight,
																							  x => x.Height, x => x.Length, x => x.Width, x => x.TaxType, x => x.ProductType, x => x.ShippingType,
																							  x=> x.Vendor, x => x.DownloadType, x => x.DownloadExpiration, x => x.HasUserAgreement).Select(x => new CsvImportMappingItem { EntityColumnName = x, IsRequired = false }));
		


			retVal.MappingItems = mappingItems.ToArray();
		

			//Read csv headers and try to auto map fields by name
			using (var reader = new CsvReader(new StreamReader(_blobStorageProvider.OpenReadOnly(fileUrl))))
			{
				reader.Configuration.Delimiter = delimiter;
				while (reader.Read())
				{
					var csvColumns = reader.FieldHeaders;
					retVal.CsvColumns = csvColumns;
					//default columns mapping
					if (csvColumns.Any())
					{
						foreach (var mappingItem in retVal.MappingItems)
						{
							var entityColumnName = mappingItem.EntityColumnName;
							var betterMatchCsvColumn = csvColumns.Select(x => new { csvColumn = x, distance = x.ComputeLevenshteinDistance(entityColumnName) })
																 .Where(x=>x.distance < 2)
																 .OrderBy(x => x.distance)
																 .Select(x=>x.csvColumn)
																 .FirstOrDefault();
							if (betterMatchCsvColumn != null)
							{
								mappingItem.CsvColumnName = betterMatchCsvColumn;
								mappingItem.CustomValue = null;
							}
						}
					}
				}
			}
			//All not mapped properties may be a product property
			retVal.PropertyCsvColumns = retVal.CsvColumns.Except(retVal.MappingItems.Where(x => x.CsvColumnName != null).Select(x => x.CsvColumnName)).ToArray();
			return Ok(retVal);
		}


		/// <summary>
		/// GET api/catalog/import/sony
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[ResponseType(typeof(ExportNotification))]
		[HttpPost]
		[Route("import")]
		public IHttpActionResult DoImport(CsvImportConfiguration importConfiguration)
		{
			var notification = new ImportNotification(CurrentPrincipal.GetCurrentUserName())
			{
				Title = "Import catalog from CSV",
				Description = "starting import...."
			};
			_notifier.Upsert(notification);

			var importJob = new CsvCatalogImportJob(_catalogService,  _categoryService, _productService, _notifier, _cacheManager, _blobStorageProvider, _skuGenerator, _pricingService, _inventoryService, _commerceService);
			BackgroundJob.Enqueue(() => importJob.DoImport(importConfiguration, notification));

			return Ok(notification);

		}


		/// <summary>
		///  GET api/catalog/importjobs/123/cancel
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id}/cancel")]
		[ResponseType(typeof(void))]
		public IHttpActionResult Cancel(string id)
		{
			return StatusCode(HttpStatusCode.NoContent);
			//var job = _jobList.FirstOrDefault(x => x.Id == id);
			//if (job != null && job.CanBeCanceled)
			//{
			//	job.CancellationToken.Cancel();
			//}

			//return StatusCode(HttpStatusCode.NoContent);
		}


    }
}
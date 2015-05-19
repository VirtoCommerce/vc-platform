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
		private readonly ExportNotification _notification;

		public CsvCatalogExportJob(ICatalogSearchService catalogSearchService,
								ICategoryService categoryService, IItemService productService,
								INotifier notifier, CacheManager cacheManager, IBlobStorageProvider blobProvider, IBlobUrlResolver blobUrlResolver)
		{
			_searchService = catalogSearchService;
			_categoryService = categoryService;
			_productService = productService;
			_notifier = notifier;
			_cacheManager = cacheManager;
			_blobStorageProvider = blobProvider;
			_blobUrlResolver = blobUrlResolver;
			_notification = new ExportNotification
			{ 
				Title = "Catalog export task",
				NotifyType = "CatalogExport",
				Description = "starting export...."
			};
		
		}

		public virtual void DoExport(string catalogId)
		{
			var memoryStream = new MemoryStream();
			var streamWriter = new StreamWriter(memoryStream);
			var productPropertyInfos = typeof(CatalogProduct).GetProperties(BindingFlags.Instance | BindingFlags.Public);

			using (var csvWriter = new CsvWriter(streamWriter))
			{
				csvWriter.Configuration.Delimiter = ";";

				//Notification
				_notification.Description = "loading products...";
				_notifier.Upsert(_notification);

				try
				{
					//Load all products to export
					var products = LoadProducts(catalogId);
					//Get a export configuration
					var exportConfiguration = GetProductExportConfiguration(products);

					//Write header
					foreach (var cfgItem in exportConfiguration)
					{
						csvWriter.WriteField(cfgItem.Key);
					}
					csvWriter.NextRecord();

					var notifyProductSizeLimit = 100;
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
						catch(Exception ex)
						{
							_notification.ErrorCount++;
							_notification.Errors.Add(ex.ToString());
							_notifier.Upsert(_notification);
						}

						//Raise notification each notifyProductSizeLimit products
						counter++;
						if (counter % notifyProductSizeLimit == 0)
						{
							_notification.ProcessedCount = counter;
							_notification.TotalCount = products.Count();
							_notification.Description = string.Format("{0} of {1} products processed", _notification.ProcessedCount, _notification.TotalCount);
							_notifier.Upsert(_notification);
						}
					}

					memoryStream.Position = 0;
					//Upload result csv to blob storage
					var uploadInfo = new UploadStreamInfo
					{
						FileName = "Catalog-" + catalogId + "-export.csv",
						FileByteStream = memoryStream,
						FolderName = "export"
					};
					var blobKey = _blobStorageProvider.Upload(uploadInfo);
					//Get a download url
					_notification.DownloadUrl = _blobUrlResolver.GetAbsoluteUrl(blobKey);
					_notification.Description = "Export finished";
				}
				catch(Exception ex)
				{
					_notification.Description = "Export error";
					_notification.ErrorCount++;
					_notification.Errors.Add(ex.ToString());
				}
				finally
				{
					_notification.Finished = DateTime.UtcNow;
					_notifier.Upsert(_notification);
				}
		
			}
			
		}

		private IEnumerable<CatalogProduct> LoadProducts(string catalogId)
		{
			var retVal = new List<CatalogProduct>();
			int maxProductCount = 50;
			int index = 0;
			var totalCount = 0;
			do
			{
				var result = _searchService.Search(new SearchCriteria { CatalogId = catalogId, GetAllCategories = true, Start = index, Count = maxProductCount, ResponseGroup = ResponseGroup.WithProducts });
				var products = _productService.GetByIds(result.Products.Select(x => x.Id).ToArray(), ItemResponseGroup.ItemLarge);
				foreach(var product in products)
				{
					retVal.Add(product);
					if (product.Variations != null)
					{
						retVal.AddRange(product.Variations);
					}
				}
				totalCount = result.TotalCount;
				index += maxProductCount;
			}
			while (index < totalCount);
			return retVal;
		}

		private Dictionary<string, Func<CatalogProduct, string>> GetProductExportConfiguration(IEnumerable<CatalogProduct> products)
		{
			var retVal = new Dictionary<string, Func<CatalogProduct, string>>();
			//Scalar Properties
			var exportScalarFields = ReflectionUtility.GetPropertyNames<CatalogProduct>(x => x.Name, x => x.Code, x => x.IsActive, x => x.IsBuyable, x => x.TrackInventory);
			retVal.AddRange(exportScalarFields.Select(x => new KeyValuePair<string, Func<CatalogProduct, string>>(x, null)));

			//Category
			retVal.Add("Category", (product) =>
			{
				if(product.CategoryId != null)
				{
					var category = _categoryService.GetById(product.CategoryId);
					var path = String.Join("/", category.Parents.Select(x => x.Name).Concat(new string[] { category.Name }));
					return path;
				}
				return String.Empty;
			});

			retVal.Add("ImageSrc", (product) =>
			{
				var image  = product.Assets.Where(x=>x.Type == ItemAssetType.Image).FirstOrDefault();
				if (image != null)
				{
					return image.Url ?? String.Empty;
				}
				return String.Empty;
			});

			retVal.Add("Review", (product) =>
			{
				var review = product.Reviews.FirstOrDefault();
				if (review != null)
				{
					return review.Content ?? String.Empty;
				}
				return String.Empty;
			});

			//Properties
			foreach (var propertyName in products.SelectMany(x => x.PropertyValues).Select(x => x.PropertyName).Distinct())
			{
				if (!retVal.ContainsKey(propertyName))
				{
					retVal.Add(propertyName, (product) =>
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
			


			return retVal;

		}

	}
}
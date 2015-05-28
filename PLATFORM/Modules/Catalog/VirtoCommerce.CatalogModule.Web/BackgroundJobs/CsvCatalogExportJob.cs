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
		}

		public virtual void DoExport(string catalogId, ExportNotification notification)
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
					var products = LoadProducts(catalogId);
					notification.TotalCount = products.Count();
					//Get a export configuration
					var exportConfiguration = GetProductExportConfiguration(products);

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
						notification.ProcessedCount = counter;
						notification.Description = string.Format("{0} of {1} products processed", notification.ProcessedCount, notification.TotalCount);
						counter++;
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

			retVal.Add("PrimaryImage", (product) =>
			{
				var image  = product.Assets.Where(x=>x.Type == ItemAssetType.Image && x.Group == "primaryimage").FirstOrDefault();
				if (image != null)
				{
					return image.Url ?? String.Empty;
				}
				return String.Empty;
			});

			retVal.Add("AltImage", (product) =>
			{
				var image = product.Assets.Where(x => x.Type == ItemAssetType.Image && x.Group != "primaryimage").FirstOrDefault();
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

			retVal.Add("SeoUrl", (product) =>
			{
				var seoInfo = product.SeoInfos.FirstOrDefault();
				if (seoInfo != null)
				{
					return seoInfo.SemanticUrl ?? String.Empty;
				}
				return String.Empty;
			});
			retVal.Add("SeoTitle", (product) =>
			{
				var seoInfo = product.SeoInfos.FirstOrDefault();
				if (seoInfo != null)
				{
					return seoInfo.PageTitle ?? String.Empty;
				}
				return String.Empty;
			});
			retVal.Add("SeoDescription", (product) =>
			{
				var seoInfo = product.SeoInfos.FirstOrDefault();
				if (seoInfo != null)
				{
					return seoInfo.MetaDescription ?? String.Empty;
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
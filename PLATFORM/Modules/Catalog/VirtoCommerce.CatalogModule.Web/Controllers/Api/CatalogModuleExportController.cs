using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using VirtoCommerce.CatalogModule.Web.BackgroundJobs;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/export")]
    public class CatalogModuleExportController : ApiController
    {
		private readonly ICatalogSearchService _searchService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _productService;
		private readonly INotifier _notifier;
		private readonly CacheManager _cacheManager;
		private readonly IBlobStorageProvider _blobStorageProvider;
		private readonly IBlobUrlResolver _blobUrlResolver;

		public CatalogModuleExportController(ICatalogSearchService catalogSearchService,
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

		/// <summary>
		/// GET api/catalog/export/sony
		/// </summary>
		/// <param name="id"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		[ResponseType(typeof(void))]
		[HttpGet]
		[Route("{catalogId}")]
		public IHttpActionResult DoExport(string catalogId)
		{
			var exportJob = new CsvCatalogExportJob(_searchService, _categoryService, _productService, _notifier, _cacheManager, _blobStorageProvider, _blobUrlResolver);
			BackgroundJob.Enqueue(() => exportJob.DoExport(catalogId));
		
			return Ok();

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
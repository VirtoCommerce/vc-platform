using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using storeModel = VirtoCommerce.Domain.Store.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/categories")]
    public class MerchandisingModuleCategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;
        private readonly ICatalogSearchService _searchService;
        private readonly IStoreService _storeService;
        private readonly CacheManager _cacheManager;
        private readonly IBlobUrlResolver _blobUrlResolver;
        public MerchandisingModuleCategoryController(ICatalogSearchService searchService, ICategoryService categoryService,
                                  IPropertyService propertyService, IStoreService storeService, CacheManager cacheManager, IBlobUrlResolver blobUrlResolver)

        {
            _storeService = storeService;
            _searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
            _cacheManager = cacheManager;
            _blobUrlResolver = blobUrlResolver;
        }

        /// <summary>
        /// Get store category by id
        /// </summary>
        /// <param name="category">Category id</param>
        /// <param name="store">Store id</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <response code="200"></response>
        /// <response code="404">Category not found</response>
		[HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("{category}")]
        public IHttpActionResult GetCategoryById(string category, string store, string language = "en-us")
        {
            var retVal = _categoryService.GetById(category);
            if (retVal != null)
                return Ok(retVal.ToWebModel(_blobUrlResolver));
            return NotFound();
        }

        /// <summary>
        /// Get store category by code
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="code">Category code</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <response code="200"></response>
        /// <response code="404">Category not found</response>
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetCategoryByCode(string store, [FromUri] string code, string language = "en-us")
        {
            var catalog = GetStoreById(store).Catalog;
            var searchCriteria = new SearchCriteria
            {
                ResponseGroup = ResponseGroup.WithCategories,
                Code = code,
                CatalogId = catalog
            };

            var result = _searchService.Search(searchCriteria);
            if (result.Categories != null && result.Categories.Any())
            {
                var category = _categoryService.GetById(result.Categories.First().Id);
                if (category != null)
                {
                    return Ok(category.ToWebModel(_blobUrlResolver));
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Get store category by SEO keyword
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="keyword">Category SEO keyword</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <response code="200"></response>
        /// <response code="404">Category not found</response>
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetCategoryByKeyword(string store, [FromUri] string keyword, string language = "en-us")
        {
            var catalog = GetStoreById(store).Catalog;
            var searchCriteria = new SearchCriteria
            {
                ResponseGroup = ResponseGroup.WithCategories,
                SeoKeyword = keyword,
                CatalogId = catalog
            };

            var result = _searchService.Search(searchCriteria);
            if (result.Categories != null && result.Categories.Any())
            {
                var category = _categoryService.GetById(result.Categories.First().Id);
                if (category != null)
                {
                    return Ok(category.ToWebModel(_blobUrlResolver));
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Search for store categories
        /// </summary>
        /// <param name="store">Store id</param>
        /// <param name="language">Culture name (default value is "en-us")</param>
        /// <param name="parentId">Parent category id</param>
        [HttpGet]
        [ResponseType(typeof(webModel.CategoryResponseCollection))]
        [ClientCache(Duration = 30)]
        [Route("search")]
        public IHttpActionResult SearchCategory(
            string store,
            string language = "en-us",
            [FromUri] string parentId = null)
        {
            var catalog = _storeService.GetById(store).Catalog;
            var criteria = new moduleModel.SearchCriteria
            {
                CatalogId = catalog,
                CategoryId = parentId,
                Start = 0,
                Count = int.MaxValue,
                HideDirectLinedCategories = true,
                ResponseGroup = moduleModel.ResponseGroup.WithCategories,
                GetAllCategories = false //string.IsNullOrEmpty(parentId)
            };
            var result = this._searchService.Search(criteria);
            var categories = result.Categories.Where(x => x.IsActive ?? true);
            return this.Ok(
                new webModel.CategoryResponseCollection
                {
                    TotalCount = categories.Count(),
                    Items = categories.Select(x => x.ToWebModel(_blobUrlResolver)).ToList()
                });
        }

        private storeModel.Store GetStoreById(string storeId)
        {
            var retVal = _storeService.GetById(storeId);
            return retVal;
        }
    }
}

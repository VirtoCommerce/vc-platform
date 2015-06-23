using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

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
        public MerchandisingModuleCategoryController(ICatalogSearchService searchService, ICategoryService categoryService,
								  IPropertyService propertyService, IStoreService storeService, CacheManager cacheManager)
      
        {
			_storeService = storeService;
            _searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
			_cacheManager = cacheManager;
        }

		[HttpGet]
		[ResponseType(typeof(webModel.Category))]
		[ClientCache(Duration = 30)]
		[Route("{category}")]
		public IHttpActionResult GetCategoryById(string category, string store, string language = "en-us")
		{
			var retVal = _categoryService.GetById(category);
			if (retVal != null)
				return Ok(retVal.ToWebModel());
			return NotFound();
		}

        /// GET: api/mp/apple/en-us/categories?code=22
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("")]
		public IHttpActionResult GetCategoryByCode(string store, [FromUri] string code, string language = "en-us")
		{
			var catalog = _storeService.GetById(store).Catalog;
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
					return Ok(category.ToWebModel());
				}
			}

			return NotFound();
		}

        /// GET: api/mp/apple/en-us/categories?keyword=apple-mp3
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("")]
		public IHttpActionResult GetCategoryByKeyword(string store, [FromUri] string keyword, string language = "en-us")
		{
			var catalog = _storeService.GetById(store).Catalog;
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
					return Ok(category.ToWebModel());
				}
			}
			return NotFound();
		}

        /// <summary>
        ///     GET: api/mp/apple/en-us/categories?parentId='22'
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="language">The language.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.ResponseCollection<webModel.Category>))]
        [ClientCache(Duration = 30)]
        [Route("")]
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
                               GetAllCategories = string.IsNullOrEmpty(parentId)
                           };
            var result = this._searchService.Search(criteria);
            var categories = result.Categories.Where(x => x.IsActive);
            return this.Ok(
                new webModel.ResponseCollection<webModel.Category>
                {
                    TotalCount = categories.Count(),
                    Items = categories.Select(x => x.ToWebModel()).ToList()
                });
        }
    }
}

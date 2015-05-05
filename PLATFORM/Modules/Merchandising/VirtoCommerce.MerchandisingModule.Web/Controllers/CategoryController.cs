using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/categories")]
	public class CategoryController : ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;
        private readonly ICatalogSearchService _searchService;
		private readonly IStoreService _storeService;

        public CategoryController(ICatalogSearchService searchService, ICategoryService categoryService,
								  IPropertyService propertyService, IStoreService storeService)
      
        {
			_storeService = storeService;
            _searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
        }
   
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("{category}")]
        public IHttpActionResult Get(string category, string store, string language = "en-us")
        {
            //var catalog = GetCatalogId(store);
            if (category != null)
            {
                var result = this._categoryService.GetById(category);
                return this.Ok(result.ToWebModel());
            }
            return this.StatusCode(HttpStatusCode.NotFound);
        }

        /// GET: api/mp/apple/en-us/categories?code=22
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetByCode(string store, [FromUri] string code, string language = "en-us")
        {
			var catalog = _storeService.GetById(store).Catalog;
			var searchCriteria = new SearchCriteria
			{
				ResponseGroup = ResponseGroup.WithCategories,
				Code = code,
				CatalogId = catalog
			};

			var result = _searchService.Search(searchCriteria);
			if(result.Categories != null && result.Categories.Any())
			{
				return this.Get(result.Categories.First().Id, catalog, language);
			}
            return this.StatusCode(HttpStatusCode.NotFound);
        }

        /// GET: api/mp/apple/en-us/categories?keyword=apple-mp3
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [ClientCache(Duration = 30)]
        [Route("")]
        public IHttpActionResult GetByKeyword(string store, [FromUri] string keyword, string language = "en-us")
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
				return this.Get(result.Categories.First().Id, catalog, language);
			}
			return this.StatusCode(HttpStatusCode.NotFound);
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
        public IHttpActionResult Search(string store, string language = "en-us", [FromUri] string parentId = null)
        {
			var catalog = _storeService.GetById(store).Catalog;
            var criteria = new moduleModel.SearchCriteria
                           {
                               CatalogId = catalog,
                               CategoryId = parentId,
                               Start = 0,
                               Count = int.MaxValue,
                               ResponseGroup = moduleModel.ResponseGroup.WithCategories,
                               GetAllCategories = string.IsNullOrEmpty(parentId)
                           };
            var result = this._searchService.Search(criteria);
            var retVal = new webModel.ResponseCollection<webModel.Category>
                         {
                             TotalCount = result.Categories.Count(),
                             Items = result.Categories.Where(x => x.IsActive).Select(x => x.ToWebModel()).ToList()
                         };

            return this.Ok(retVal);
        }

    }
}

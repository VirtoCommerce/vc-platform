using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{store}/{language}/categories")]
    public class CategoryController : BaseController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;
        private readonly Func<IFoundationCatalogRepository> _foundationCatalogRepositoryFactory;
        private readonly Func<IFoundationAppConfigRepository> _foundationAppConfigRepFactory;

        public CategoryController(ICatalogSearchService searchService,
                                  ICategoryService categoryService,
                                  IPropertyService propertyService,
                                  Func<IFoundationCatalogRepository> foundationCatalogRepositoryFactory,
                                  Func<IFoundationAppConfigRepository> foundationAppConfigRepFactory,
                                  Func<IStoreRepository> storeRepository)
            : base(storeRepository)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
            _foundationCatalogRepositoryFactory = foundationCatalogRepositoryFactory;
            _foundationAppConfigRepFactory = foundationAppConfigRepFactory;
        }


        /// <summary>
        /// GET: api/mp/apple/en-us/categories?parentId='22'
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="language">The language.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.ResponseCollection<webModel.Category>))]
        [Route("")]
        public IHttpActionResult Search(string store, string language = "en-us", [FromUri]string parentId = null)
        {
            var catalog = GetCatalogId(store);
            var criteria = new moduleModel.SearchCriteria
            {
                CatalogId = catalog,
                CategoryId = parentId,
                Start = 0,
                Count = int.MaxValue,
                ResponseGroup = moduleModel.ResponseGroup.WithCategories,
                GetAllCategories = string.IsNullOrEmpty(parentId)
            };
            var result = _searchService.Search(criteria);
            var retVal = new webModel.ResponseCollection<webModel.Category>
            {
                TotalCount = result.Categories.Count(),
                Items = result.Categories.Where(x => x.IsActive).Select(x => x.ToWebModel()).ToList()
            };

            return Ok(retVal);
        }


        /// GET: api/mp/apple/en-us/categories?code='22'
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [Route("")]
        public IHttpActionResult GetByCode(string store, [FromUri]string code, string language = "en-us")
        {
            var catalog = GetCatalogId(store);
            using (var repository = _foundationCatalogRepositoryFactory())
            {
                var categoryId = repository.Categories.Where(x => x.CatalogId == catalog && x.Code == code).Select(x => x.CategoryId).FirstOrDefault();
                if (categoryId != null)
                {
                    return Get(categoryId, catalog, language);
                }
            }
            return StatusCode(HttpStatusCode.NotFound);
        }

        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [Route("{category}")]
        public IHttpActionResult Get(string category, string store, string language = "en-us")
        {
            //var catalog = GetCatalogId(store);
            if (category != null)
            {

                var result = _categoryService.GetById(category);
                if (result == null)
                {
                    //Lets treat categoryId as slug
                    using (var appConfigRepo = _foundationAppConfigRepFactory())
                    {
                        var keyword = appConfigRepo.SeoUrlKeywords.FirstOrDefault(x => x.KeywordType == (int)SeoUrlKeywordTypes.Category
                            && x.Keyword.Equals(category, StringComparison.InvariantCultureIgnoreCase));

                        if (keyword != null)
                        {
                            result = _categoryService.GetById(keyword.KeywordValue);
                        }
                    }
                }


                if (result != null)
                {
                    //need seo info for parents
                    var keywords = new List<SeoUrlKeyword>();
                    if (result.Parents != null)
                    {
                        using (var appConfigRepo = _foundationAppConfigRepFactory())
                        {
                            foreach (var parent in result.Parents)
                            {
                                keywords.AddRange(appConfigRepo.GetAllSeoInformation(parent.Id));
                            }
                        }
                    }

                    return Ok(result.ToWebModel(keywords.ToArray()));
                }
            }
            return StatusCode(HttpStatusCode.NotFound);
        }


    }
}

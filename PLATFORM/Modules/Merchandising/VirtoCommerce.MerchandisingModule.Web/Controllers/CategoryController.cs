using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.MerchandisingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/categories")]
    public class CategoryController : BaseController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly Func<IFoundationAppConfigRepository> _foundationAppConfigRepFactory;
        private readonly Func<IFoundationCatalogRepository> _foundationCatalogRepositoryFactory;
        private readonly IPropertyService _propertyService;
        private readonly ICatalogSearchService _searchService;

        #endregion

        #region Constructors and Destructors

        public CategoryController(
            ICatalogSearchService searchService,
            ICategoryService categoryService,
            IPropertyService propertyService,
            Func<IFoundationCatalogRepository> foundationCatalogRepositoryFactory,
            Func<IFoundationAppConfigRepository> foundationAppConfigRepFactory,
            Func<IStoreRepository> storeRepository,
            ISettingsManager settingsManager,
            ICacheRepository cache)
            : base(storeRepository, settingsManager, cache)
        {
            this._searchService = searchService;
            this._categoryService = categoryService;
            this._propertyService = propertyService;
            this._foundationCatalogRepositoryFactory = foundationCatalogRepositoryFactory;
            this._foundationAppConfigRepFactory = foundationAppConfigRepFactory;
        }

        #endregion

        #region Public Methods and Operators

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
            var catalog = this.GetCatalogId(store);
            using (var repository = this._foundationCatalogRepositoryFactory())
            {
                var categoryId =
                    repository.Categories.Where(x => x.CatalogId == catalog && x.Code == code)
                        .Select(x => x.CategoryId)
                        .FirstOrDefault();
                if (categoryId != null)
                {
                    return this.Get(categoryId, catalog, language);
                }
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
            //var catalog = GetCatalogId(store);
            SeoUrlKeyword urlKeyword = null;
            using (var appConfigRepo = this._foundationAppConfigRepFactory())
            {
                urlKeyword =
                    appConfigRepo.SeoUrlKeywords.FirstOrDefault(
                        x => x.KeywordType == (int)SeoUrlKeywordTypes.Category
                            && x.Keyword.Equals(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (urlKeyword != null)
            {
                var result = this._categoryService.GetById(urlKeyword.KeywordValue);

                if (result != null)
                {
                    //need seo info for parents
                    var keywords = new List<SeoUrlKeyword>();
                    /*
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
                     * */

                    return this.Ok(result.ToWebModel());
                }
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
            var catalog = this.GetCatalogId(store);
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

        #endregion
    }
}

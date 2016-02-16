using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.CatalogModule.Web.Security;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CatalogModule.Web.Binders;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/categories")]
    public class CatalogModuleCategoriesController : CatalogBaseController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly ICatalogService _catalogService;
		private readonly IBlobUrlResolver _blobUrlResolver;

        public CatalogModuleCategoriesController(ICatalogSearchService searchService, ICategoryService categoryService,  ICatalogService catalogService, IBlobUrlResolver blobUrlResolver, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            :base(securityService, permissionScopeService)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _catalogService = catalogService;
			_blobUrlResolver = blobUrlResolver;
        }


        /// <summary>
        /// Gets category by id.
        /// </summary>
        /// <param name="id">Category id.</param>
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var category = GetCategoriesByIds(new[] { id }).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        /// <summary>
        /// Gets categories by ids
        /// </summary>
        /// <param name="ids">Categories ids</param>
        ///<param name="respGroup">Response group.</param>
        //Because Swagger generated API client passed arrays as joined string need parse query string by binder
        [HttpGet]
        [Route("")]
        public webModel.Category[] GetCategoriesByIds([ModelBinder(typeof(IdsStringArrayBinder))] string[] ids, [FromUri] coreModel.CategoryResponseGroup respGroup = coreModel.CategoryResponseGroup.Full)
        {
            var categories = _categoryService.GetByIds(ids, respGroup);

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, categories);

            var retVal = categories.Select(x => x.ToWebModel(_blobUrlResolver));
            foreach (var category in retVal)
            {
                category.SecurityScopes = base.GetObjectPermissionScopeStrings(category);
            }

            return retVal.ToArray();
        }

        /// <summary>
        /// Gets the template for a new category.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="parentCategoryId">The parent category id. (Optional)</param>
        [HttpGet]
        [Route("~/api/catalog/{catalogId}/categories/newcategory")]
        [ResponseType(typeof(webModel.Category))]
        public IHttpActionResult GetNewCategory(string catalogId, [FromUri]string parentCategoryId = null)
        {
            var retVal = new webModel.Category
            {
                ParentId = parentCategoryId,
                CatalogId = catalogId,
                Catalog = _catalogService.GetById(catalogId).ToWebModel(),
                Code = Guid.NewGuid().ToString().Substring(0, 5),
                SeoInfos = new List<SeoInfo>(),
				IsActive = true
            };

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, retVal.ToModuleModel());
            retVal.SecurityScopes = base.GetObjectPermissionScopeStrings(retVal.ToModuleModel());

            return Ok(retVal);
        }


        /// <summary>
        /// Creates or updates the specified category.
        /// </summary>
        /// <remarks>If category.id is null, a new category is created. It's updated otherwise</remarks>
        /// <param name="category">The category.</param>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult CreateOrUpdateCategory(webModel.Category category)
        {
            var coreCategory = category.ToModuleModel();
            if (coreCategory.Id == null)
			{
				if (coreCategory.SeoInfos == null || !coreCategory.SeoInfos.Any())
				{
					var slugUrl = category.Name.GenerateSlug();
					if (!String.IsNullOrEmpty(slugUrl))
					{
						var catalog = _catalogService.GetById(category.CatalogId);
						var defaultLanguage = catalog.Languages.First(x => x.IsDefault).LanguageCode;
						coreCategory.SeoInfos = new SeoInfo[] { new SeoInfo { LanguageCode = defaultLanguage, SemanticUrl = slugUrl } };
					}
				}

                base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, coreCategory);

                var retVal = _categoryService.Create(coreCategory).ToWebModel(_blobUrlResolver);
				retVal.Catalog = null;
				return Ok(retVal);
			}
            else
            {
                base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Update, coreCategory);

                _categoryService.Update(new[] { coreCategory });
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        
        /// <summary>
        /// Deletes the specified categories by id.
        /// </summary>
        /// <param name="ids">The categories ids.</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Delete([FromUri]string[] ids)
        {
            var categories = _categoryService.GetByIds(ids, Domain.Catalog.Model.CategoryResponseGroup.WithParents);
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Delete, categories);

            _categoryService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

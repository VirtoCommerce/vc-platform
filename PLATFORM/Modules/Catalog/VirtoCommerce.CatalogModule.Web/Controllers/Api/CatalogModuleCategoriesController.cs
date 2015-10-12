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

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/categories")]
    public class CatalogModuleCategoriesController : CatalogBaseController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;
        private readonly ICatalogService _catalogService;
		private readonly IBlobUrlResolver _blobUrlResolver;

        public CatalogModuleCategoriesController(ICatalogSearchService searchService, ICategoryService categoryService, IPropertyService propertyService, 
                                                 ICatalogService catalogService, IBlobUrlResolver blobUrlResolver, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            :base(securityService, permissionScopeService)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
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
            var category = _categoryService.GetById(id);

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, category);

            if (category == null)
            {
                return NotFound();
            }
            var allCategoryProperties = _propertyService.GetCategoryProperties(id);
            var retVal = category.ToWebModel(_blobUrlResolver, allCategoryProperties);

            retVal.SecurityScopes = base.GetObjectPermissionScopeStrings(category);

            return Ok(retVal);
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
            var categories = ids.Select(x => _categoryService.GetById(x)).ToArray();
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Delete, categories);

            _categoryService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

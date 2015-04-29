using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/categories")]
    [CheckPermission(Permission = PredefinedPermissions.CategoriesManage)]
    public class CategoriesController : ApiController
    {
        private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;
        private readonly ICatalogService _catalogService;

        public CategoriesController(ICatalogSearchService searchService,
                                    ICategoryService categoryService,
                                    IPropertyService propertyService, ICatalogService catalogService)
        {
            _searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
            _catalogService = catalogService;
        }

        // GET: api/catalog/categories/5
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }
            var allCategoryProperties = _propertyService.GetCategoryProperties(id);
            var retVal = category.ToWebModel(allCategoryProperties);
            return Ok(retVal);
        }

        // GET: api/catalog/apple/categories/newcategory&parentCategoryId='ddd'"
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
                SeoInfos = new List<webModel.SeoInfo>()
            };

            return Ok(retVal);
        }


        // POST:  api/catalog/categories
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Post(webModel.Category category)
        {
            var moduleObj = category.ToModuleModel();
            if (moduleObj.Id == null)
            {
                var retVal = _categoryService.Create(moduleObj).ToWebModel();
                retVal.Catalog = null;
                return Ok(retVal);
            }
            else
            {
                _categoryService.Update(new[] { moduleObj });
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        // POST: api/catalog/categories/5
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Delete([FromUri]string[] ids)
        {
            _categoryService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

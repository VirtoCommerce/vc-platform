using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Services;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog/categories")]
    public class CategoriesController : ApiController
    {
		private readonly ICatalogSearchService _searchService;
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;

		public CategoriesController(ICatalogSearchService searchService,
								    ICategoryService categoryService,
									IPropertyService propertyService)
        {
			_searchService = searchService;
            _categoryService = categoryService;
            _propertyService = propertyService;
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
                Name = "New category",
				ParentId = parentCategoryId,
                CatalogId = catalogId,
                Code = Guid.NewGuid().ToString().Substring(0, 5)
            };

            retVal = _categoryService.Create(retVal.ToModuleModel()).ToWebModel();

            return Ok(retVal);
        }


		// POST:  api/catalog/categories
        [HttpPost]
        [ResponseType(typeof(webModel.Category))]
		[Route("")]
        public IHttpActionResult Post(webModel.Category category)
        {
            UpdateCategory(category);
			var retVal = _categoryService.GetById(category.Id);
			return Ok(retVal);
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
        
        private void UpdateCategory(webModel.Category category)
        {
            var moduleCategory = category.ToModuleModel();
            _categoryService.Update(new moduleModel.Category[] { moduleCategory });
        }
    }
}

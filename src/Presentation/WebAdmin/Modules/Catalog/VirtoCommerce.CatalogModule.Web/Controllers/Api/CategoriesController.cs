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

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IPropertyService _propertyService;

        public CategoriesController(ICategoryService categoryService, IPropertyService propertyService)
        {
            _categoryService = categoryService;
            _propertyService = propertyService;
        }

        // GET: api/Categories/5
        [ResponseType(typeof(webModel.Category))]
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

        // GET: api/Categories/getnewcategory
        [HttpGet]
        [ResponseType(typeof(webModel.Category))]
        public IHttpActionResult GetNewCategory(string catalogId, string parentCategoryId = null)
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

	
        // POST: api/categories
        [HttpPost]
        [ResponseType(typeof(webModel.Category))]
        public IHttpActionResult Post(webModel.Category category)
        {

            UpdateCategory(category);

            return CreatedAtRoute("DefaultApi", new { id = category.Id }, category);
        }


        // POST: api/categories/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(string[] ids)
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

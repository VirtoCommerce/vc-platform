using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Catalog.Services;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog/properties")]
    public class PropertiesController : ApiController
    {
        private readonly IPropertyService _propertyService;
		private readonly ICategoryService _categoryService;
		private readonly ICatalogService _catalogService;

		public PropertiesController(IPropertyService propertyService,
									ICategoryService categoryService, ICatalogService catalogService)
        {
            _propertyService = propertyService;
			_categoryService = categoryService;
			_catalogService = catalogService;
		
        }

        // GET api/catalog/properties/11/values
        [HttpGet]
		[Route("{propertyId}/values")]
		[ResponseType(typeof(webModel.PropertyValue[]))]
        public IHttpActionResult GetPropertyValues(string propertyId, [FromUri]string keyword = null)
        {
			var retVal = new List<webModel.PropertyValue>();
			//Need return propValue because it more convenient for ui
			var dictValues = _propertyService.SearchDictionaryValues(propertyId, keyword);
			foreach(var dictValue in dictValues)
			{
				var propValue = new webModel.PropertyValue
				{
					Value = dictValue.Value,
					ValueId = dictValue.Id,
					LanguageCode = dictValue.LanguageCode
				};
				retVal.Add(propValue);
			}
			return Ok(retVal.ToArray());
        }

		// GET: api/catalog/properties/11
		[HttpGet]
		[Route("{propertyId}")]
		[ResponseType(typeof(webModel.Property))]
        public IHttpActionResult Get(string propertyId)
        {
			var property = _propertyService.GetById(propertyId);
			if (property == null)
			{
				return NotFound();
			}
			var retVal = property.ToWebModel();
		    retVal.IsManageable = true;
			return Ok(retVal);
        }

		// GET: api/catalog/apple/properties/getnew
		[HttpGet]
		[Route("~/api/catalog/{catalogId}/properties/getnew")]
		[ResponseType(typeof(webModel.Property))]
		public IHttpActionResult GetNewCatalogProperty(string catalogId)
		{
			var catalog = _catalogService.GetById(catalogId);
			var retVal = new webModel.Property
			{
				Id = Guid.NewGuid().ToString(),
				IsNew = true,
				CatalogId = catalog.Id,
				Catalog = catalog.ToWebModel(),
				Name = "new property",
				Type = moduleModel.PropertyType.Catalog,
				ValueType = moduleModel.PropertyValueType.ShortText,
				DictionaryValues = new List<webModel.PropertyDictionaryValue>(),
				Attributes = new List<webModel.PropertyAttribute>()
			};

			return Ok(retVal);
		}

		// GET: api/catalog/categories/apple/properties/getnew
        [HttpGet]
		[Route("~/api/catalog/categories/{categoryId}/properties/getnew")]
		[ResponseType(typeof(webModel.Property))]
        public IHttpActionResult GetNewCategoryProperty(string categoryId)
        {
			var category = _categoryService.GetById(categoryId);
			var retVal = new webModel.Property
			{
				Id = Guid.NewGuid().ToString(),
				IsNew = true,
				CategoryId = categoryId,
				Category = category.ToWebModel(),
				CatalogId = category.CatalogId,
				Catalog = category.Catalog.ToWebModel(),
				Name = "new property",
				Type = moduleModel.PropertyType.Category,
				ValueType = moduleModel.PropertyValueType.ShortText,
				DictionaryValues = new List<webModel.PropertyDictionaryValue>(),
				Attributes = new List<webModel.PropertyAttribute>()
			};
		
            return Ok(retVal);
        }

		// POST: api/catalog/properties
        [HttpPost]
		[Route("")]
        [ResponseType(typeof(void))]
		public IHttpActionResult Post(webModel.Property property)
        {
			var moduleProperty = property.ToModuleModel();
		
			if (property.IsNew)
			{
				_propertyService.Create(moduleProperty);
			}
			else
			{
				_propertyService.Update(new moduleModel.Property[] { moduleProperty });
			}

		    return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/catalog/properties?id=222
		[HttpDelete]
		[Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(string id)
        {
            _propertyService.Delete(new string[] { id });
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
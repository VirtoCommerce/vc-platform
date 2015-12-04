using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Platform.Core.Security;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.CatalogModule.Web.Security;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
	[RoutePrefix("api/catalog/properties")]
    public class CatalogModulePropertiesController : CatalogBaseController
    {
        private readonly IPropertyService _propertyService;
		private readonly ICategoryService _categoryService;
		private readonly ICatalogService _catalogService;

		public CatalogModulePropertiesController(IPropertyService propertyService, ICategoryService categoryService, ICatalogService catalogService, 
                                                 ISecurityService securityService, IPermissionScopeService permissionScopeService)
            :base(securityService, permissionScopeService)
        {
            _propertyService = propertyService;
			_categoryService = categoryService;
			_catalogService = catalogService;
		
        }

        
        /// <summary>
        /// Gets all dictionary values that specified property can have.
        /// </summary>
        /// <param name="propertyId">The property id.</param>
        /// <param name="keyword">The keyword. (Optional)</param>
        /// <returns></returns>
        [HttpGet]
		[Route("{propertyId}/values")]
		[ResponseType(typeof(webModel.PropertyValue[]))]
        public IHttpActionResult GetPropertyValues(string propertyId, [FromUri]string keyword = null)
        {
            //Need to return PropertyValue as it's more convenient in UI
			var retVal = new List<webModel.PropertyValue>();
			var dictValues = _propertyService.SearchDictionaryValues(propertyId, keyword);
			foreach(var dictValue in dictValues)
			{
				var propValue = new webModel.PropertyValue
				{
					Value = dictValue.Value,
					Alias = dictValue.Alias,
					ValueId = dictValue.Id,
					LanguageCode = dictValue.LanguageCode
				};
				retVal.Add(propValue);
			}
			return Ok(retVal.ToArray());
        }


        /// <summary>
        /// Gets property metainformation by id.
        /// </summary>
        /// <param name="propertyId">The property id.</param>
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
            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Read, property);

			var retVal = property.ToWebModel();
		    retVal.IsManageable = true;
			return Ok(retVal);
        }

		
        /// <summary>
        /// Gets the template for a new catalog property.
        /// </summary>
        /// <param name="catalogId">The catalog id.</param>
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
				Attributes = new List<webModel.PropertyAttribute>(),
				DisplayNames = catalog.Languages.Select(x => new moduleModel.PropertyDisplayName { LanguageCode = x.LanguageCode }).ToList()
			};

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, retVal.ToModuleModel());

            return Ok(retVal);
		}


        /// <summary>
        /// Gets the template for a new category property.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        [HttpGet]
		[Route("~/api/catalog/categories/{categoryId}/properties/getnew")]
		[ResponseType(typeof(webModel.Property))]
        public IHttpActionResult GetNewCategoryProperty(string categoryId)
        {
			var category = _categoryService.GetById(categoryId, Domain.Catalog.Model.CategoryResponseGroup.Info);
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
				Attributes = new List<webModel.PropertyAttribute>(),
				DisplayNames = category.Catalog.Languages.Select(x => new moduleModel.PropertyDisplayName { LanguageCode = x.LanguageCode }).ToList()
			};

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, retVal.ToModuleModel());

            return Ok(retVal);
        }


        /// <summary>
        /// Creates or updates the specified property.
        /// </summary>
        /// <remarks>If property.IsNew == True, a new property is created. It's updated otherwise</remarks>
        /// <param name="property">The property.</param>
        [HttpPost]
		[Route("")]
        [ResponseType(typeof(void))]
		public IHttpActionResult CreateOrUpdateProperty(webModel.Property property)
        {
			var moduleProperty = property.ToModuleModel();
		
			if (property.IsNew)
			{
                base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Create, moduleProperty);

                _propertyService.Create(moduleProperty);
			}
			else
			{
                base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Update, moduleProperty);

                _propertyService.Update(new moduleModel.Property[] { moduleProperty });
			}

		    return StatusCode(HttpStatusCode.NoContent);
        }

        
        /// <summary>
        /// Deletes property by id.
        /// </summary>
        /// <param name="id">The property id.</param>
        /// <returns></returns>
		[HttpDelete]
		[Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(string id)
        {
            var property = _propertyService.GetById(id);

            base.CheckCurrentUserHasPermissionForObjects(CatalogPredefinedPermissions.Delete, property);

            _propertyService.Delete(new [] { id });
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
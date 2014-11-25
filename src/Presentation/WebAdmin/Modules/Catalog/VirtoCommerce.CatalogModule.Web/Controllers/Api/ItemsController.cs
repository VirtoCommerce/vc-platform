using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Services;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class ItemsController : ApiController
    {
        private readonly IItemService _itemsService;
		private readonly IPropertyService _propertyService;
    
        public ItemsController(IItemService itemsService, IPropertyService propertyService)
        {
            _itemsService = itemsService;
			_propertyService = propertyService;
        }

        // GET: api/items/5
		[ResponseType(typeof(webModel.Product))]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var item = _itemsService.GetById(id, moduleModel.ItemResponseGroup.ItemLarge);
            if (item == null)
            {
                return NotFound();
            }

			moduleModel.Property[] properties = null;
			if (item.CategoryId != null)
			{
				properties = _propertyService.GetCategoryProperties(item.CategoryId);
			}
			var retVal = item.ToWebModel(properties);
         
            return Ok(retVal);
        }

        // GET: api/items/getnewitem
        [HttpGet]
		[ResponseType(typeof(webModel.Product))]
        public IHttpActionResult GetNewItem(string catalogId = null, string categoryId = null)
        {
			var retVal = new webModel.Product
			{
				CategoryId = categoryId,
				CatalogId = catalogId
			};
		
			if (categoryId != null)
			{
				retVal.Properties = _propertyService.GetCategoryProperties(categoryId).Select(x=>x.ToWebModel()).ToList();

                foreach (var property in retVal.Properties)
                {
                    property.Values = new List<webModel.PropertyValue>();
                    property.IsManageable = true;
                    property.IsReadOnly = property.Type == webModel.PropertyType.Category;
                }
			}
			return Ok(retVal);
        }

        // GET: api/items/getnewvariation
        [HttpGet]
		[ResponseType(typeof(webModel.Product))]
        public IHttpActionResult GetNewVariation(string itemId)
        {
			var product = _itemsService.GetById(itemId, moduleModel.ItemResponseGroup.ItemLarge);
			if (product == null)
            {
                return NotFound();
            }

			moduleModel.Property[] allCategoryProperties = null;
			if (product.CategoryId != null)
			{
                allCategoryProperties = _propertyService.GetCategoryProperties(product.CategoryId);
			}

			var mainWebProduct = product.ToWebModel(allCategoryProperties);

			var newVariation = new webModel.Product
			{
			    Name = product.Name,
			    CategoryId = product.CategoryId,
			    CatalogId = product.CatalogId,
			    TitularItemId = product.MainProductId ?? itemId,
                IsVariation = true,
			    Properties = mainWebProduct.Properties.Where(x => x.Type == webModel.PropertyType.Product 
                    || x.Type == webModel.PropertyType.Variation).ToList(),
			};

            foreach (var property in newVariation.Properties)
            {
                //Need to generated new ids
                foreach (var val in property.Values.ToArray())
                {
                    val.Id = Guid.NewGuid().ToString();
                }

                // Mark variation property as required
                if (property.Type == webModel.PropertyType.Variation)
                {
                    property.Required = true;
                }

                property.IsManageable = true;
                property.IsReadOnly = property.Type == webModel.PropertyType.Category;
            }

         
			//var retVal = _itemsService.Create(newVariation.ToModuleModel()).ToWebModel();
            return Ok(newVariation);
        }

		[HttpPost]
		[ResponseType(typeof(void))]
		public IHttpActionResult Post(webModel.Product product)
		{
			var updatedProduct = UpdateProduct(product);
		    if (updatedProduct != null)
		    {
                return Ok(updatedProduct);
		    }
			return StatusCode(HttpStatusCode.NoContent);
		}

		[HttpPost]
		[ResponseType(typeof(void))]
		public IHttpActionResult Delete(string[] ids)
		{
			_itemsService.Delete(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}

		private moduleModel.CatalogProduct UpdateProduct(webModel.Product product)
		{
			var moduleProduct = product.ToModuleModel();
			if (moduleProduct.Id == null)
			{
				return _itemsService.Create(moduleProduct);
			}
			else
			{
				_itemsService.Update(new[] { moduleProduct });
			}

		    return null;
		}
    }
}
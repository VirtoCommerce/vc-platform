using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Security;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    [RoutePrefix("api/catalog/products")]
    public class ProductsController : ApiController
    {
        private readonly IItemService _itemsService;
        private readonly IPropertyService _propertyService;
		private readonly IBlobUrlResolver _blobUrlResolver;

		public ProductsController(IItemService itemsService, IPropertyService propertyService, IBlobUrlResolver blobUrlResolver)
        {
            _itemsService = itemsService;
            _propertyService = propertyService;
			_blobUrlResolver = blobUrlResolver;
        }

        // GET: api/catalog/products/5
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var item = _itemsService.GetById(id, moduleModel.ItemResponseGroup.ItemLarge);
            if (item == null)
            {
                return NotFound();
            }

			var properties = GetAllCatalogProperies(item.CatalogId, item.CategoryId);
			var retVal = item.ToWebModel(_blobUrlResolver, properties);

            return Ok(retVal);
        }

        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("~/api/catalog/{catalogId}/products/getnew")]
        [CheckPermission(Permission = PredefinedPermissions.ItemsManage)]
        public IHttpActionResult GetNewProduct(string catalogId)
        {
            return GetNewProduct(catalogId, null);
        }

        // GET: /api/catalog/apple/categories/new category/products/getnew
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("~/api/catalog/{catalogId}/categories/{categoryId}/products/getnew")]
        [CheckPermission(Permission = PredefinedPermissions.ItemsManage)]
        public IHttpActionResult GetNewProduct(string catalogId, string categoryId)
        {
            var retVal = new webModel.Product
            {
                CategoryId = categoryId,
                CatalogId = catalogId,
				IsActive = true
            };


			if (catalogId != null)
            {
				var properites = GetAllCatalogProperies(catalogId, categoryId);
				retVal.Properties = properites.Select(x => x.ToWebModel()).ToList();

                foreach (var property in retVal.Properties)
                {
                    property.Values = new List<webModel.PropertyValue>();
                    property.IsManageable = true;
					property.IsReadOnly = property.Type != moduleModel.PropertyType.Product && property.Type != moduleModel.PropertyType.Variation;
                }
            }
            return Ok(retVal);
        }

        // GET: /api/catalog/products/121/getnewvariation
        [HttpGet]
        [ResponseType(typeof(webModel.Product))]
        [Route("{productId}/getnewvariation")]
        [CheckPermission(Permission = PredefinedPermissions.ItemsManage)]
        public IHttpActionResult GetNewVariation(string productId)
        {
            var product = _itemsService.GetById(productId, moduleModel.ItemResponseGroup.ItemLarge);
            if (product == null)
            {
                return NotFound();
            }

			var properties = GetAllCatalogProperies(product.CatalogId, product.CategoryId);
		    var mainWebProduct = product.ToWebModel(_blobUrlResolver, properties);

            var newVariation = new webModel.Product
            {
                Name = product.Name,
                CategoryId = product.CategoryId,
                CatalogId = product.CatalogId,
                TitularItemId = product.MainProductId ?? productId,
				Properties = mainWebProduct.Properties.Where(x => x.Type == moduleModel.PropertyType.Product
					|| x.Type == moduleModel.PropertyType.Variation).ToList(),
            };

            foreach (var property in newVariation.Properties)
            {
                //Need to generated new ids
                foreach (var val in property.Values.ToArray())
                {
                    val.Id = Guid.NewGuid().ToString();
                }

                // Mark variation property as required
				if (property.Type == moduleModel.PropertyType.Variation)
                {
                    property.Required = true;
                }

                property.IsManageable = true;
				property.IsReadOnly = property.Type != moduleModel.PropertyType.Product && property.Type != moduleModel.PropertyType.Variation;
            }


            //var retVal = _itemsService.Create(newVariation.ToModuleModel()).ToWebModel();
            return Ok(newVariation);
        }

        // POST: /api/catalog/products
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.ItemsManage)]
        public IHttpActionResult Update(webModel.Product product)
        {
            var updatedProduct = UpdateProduct(product);
            if (updatedProduct != null)
            {
                return Ok(updatedProduct);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: /api/catalog/products?ids=21
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        [CheckPermission(Permission = PredefinedPermissions.ItemsManage)]
        public IHttpActionResult Delete([FromUri] string[] ids)
        {
            _itemsService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

		private moduleModel.Property[] GetAllCatalogProperies(string catalogId, string categoryId)
		{
			if (catalogId == null)
				throw new ArgumentNullException("catalogId");

			moduleModel.Property[] retVal = null;
			if (!String.IsNullOrEmpty(categoryId))
			{
				retVal = _propertyService.GetCategoryProperties(categoryId);
			}
			else
			{
				retVal = _propertyService.GetCatalogProperties(catalogId);
			}
			return retVal;
		}

        private moduleModel.CatalogProduct UpdateProduct(webModel.Product product)
        {
            var moduleProduct = product.ToModuleModel(_blobUrlResolver);
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
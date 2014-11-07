using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using VirtoCommerce.MerchandisingModule.Model;
using VirtoCommerce.MerchandisingModule.Services;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/merch/products")]
    public class MerchantItemsController : ApiController
    {
        private readonly IMerchantItemService _itemService = null;

        public MerchantItemsController(IMerchantItemService itemService)
        {
            _itemService = itemService;
        }

        #region Product Create/Update/Delete
        [HttpPost]
        [Route("{categoryId}")]
        public HttpResponseMessage CreateProduct(string categoryId, Product product)
        {
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product cannot be null");
            }

            try
            {
                _itemService.Create(categoryId, product);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPatch]
        [Route("")]
        public HttpResponseMessage PatchProduct(string caetgoryId, Delta<Product> product)
        {
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product cannot be null");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                _itemService.Update(product);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage DeleteProduct(string id)
        {
            if (id == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product Id cannot be null");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                _itemService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        #endregion

        [HttpPost]
        public HttpResponseMessage CreateVariation(string categoryId, string productId, ProductVariation variation)
        {
            if (variation == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product cannot be null");
            }

            try
            {
                _itemService.Create(categoryId, productId, variation);
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public Product GetProduct(string id, string groups = "")
        {
            return _itemService.GetItem<Product>(id, ItemResponseGroups.ItemLarge);
        }

        #region Product Images Create/Update
        [HttpGet]
        [Route("{id}/images")]
        public ItemImage[] GetProductImages(string id)
        {
            return _itemService.GetItem<Product>(id, ItemResponseGroups.ItemAssets).Images;
        }
        #endregion
    }
}

using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Framework.Web.Notification;

#region Google usings
using Google.Apis.ShoppingContent.v2;
using GoogleShopping.MerchantModule.Web.Providers;
using GoogleShopping.MerchantModule.Web.Services;
using Google.Apis.ShoppingContent.v2.Data;
using GoogleShopping.MerchantModule.Web.Converters;
using GoogleShopping.MerchantModule.Web.Helpers.Interfaces;
#endregion


namespace GoogleShopping.MerchantModule.Web.Controllers.Api
{
    [RoutePrefix("api/g")]
    public class GoogleShoppingController : ApiController
    {
        private const string _accessTokenPropertyName = "Google.Shopping.Credentials.AccessToken";
        private readonly IGoogleProductProvider _productProvider;
        private readonly IShopping _settingsManager;
        private readonly INotifier _notifier;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ShoppingContentService _contentService;

        public GoogleShoppingController(
            IShopping settingsManager, 
            IGoogleProductProvider productProvider, 
            INotifier notifier, 
            IDateTimeProvider dateTimeProvider,
            IGoogleContentServiceProvider googleContentServiceProvider)
        {
            _settingsManager = settingsManager;
            _productProvider = productProvider;
            _notifier = notifier;
            _dateTimeProvider = dateTimeProvider;
            _contentService = googleContentServiceProvider.GetShoppingContentService();
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("products/sync/{productId}")]
        public IHttpActionResult SyncProduct(string productId)
        {
            var products = _productProvider.GetProductUpdates(new[] { productId });
            products.ForEach(product => _contentService.Products.Insert(product, _settingsManager.MerchantId).Execute());

            return Ok();
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("products/getstatus")]
        public IHttpActionResult GetProductsStatus()
        {
            var response = _contentService.Productstatuses.List(_settingsManager.MerchantId).Execute();

            return Ok(response);
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("products/sync/outdated")]
        public IHttpActionResult UpdateOutdatedProducts()
        {
            var response = _contentService.Productstatuses.List(_settingsManager.MerchantId).Execute();

            var outdated = GetOutdatedProducts(response.Resources);
            if (outdated != null && outdated.Any())
            {
                var products = _productProvider.GetProductUpdates(outdated);
                products.ForEach(product => _contentService.Products.Insert(product, _settingsManager.MerchantId).Execute());
                return Ok();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("products/sync/batch/outdated")]
        public IHttpActionResult BatchOutdatedProducts()
        {
            var response = _contentService.Productstatuses.List(_settingsManager.MerchantId).Execute();

            var outdated = GetOutdatedProducts(response.Resources);
            if (outdated != null && outdated.Any())
            {
                var products = _productProvider.GetProductsBatchRequest(outdated);
                products.Entries.ForEach(item => item.MerchantId = _settingsManager.MerchantId);
                var res = _contentService.Products.Custombatch(products).Execute();
                return Ok(res);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("products/sync/batch/{catalogId}")]
        public IHttpActionResult BatchAllProducts(string catalogId)
        {
            var products = _productProvider.GetCatalogProductsBatchRequest(catalogId);
            if (products == null || !products.Entries.Any())
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            products.Entries.ForEach(item => item.MerchantId = _settingsManager.MerchantId);
            var res = _contentService.Products.Custombatch(products).Execute();
            return Ok(res);
        }

        private ICollection<string> GetOutdatedProducts(IEnumerable<ProductStatus> productStatuses)
        {
            ICollection<string> outdatedProductIds = null;

            productStatuses.ForEach(status =>
            {
                var converted = status.ToModuleModel();
                if (_dateTimeProvider.CurrentUtcDateTime > converted.ExpirationDate)
                {
                    if (outdatedProductIds == null)
                        outdatedProductIds = new Collection<string>();
                    outdatedProductIds.Add(converted.ProductId);
                }
            });

            return outdatedProductIds;
        }
    }
}

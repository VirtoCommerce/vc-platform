using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Google.Apis.Services;
using Google.Apis.ShoppingContent.v2;
using Google.Apis.ShoppingContent.v2.Data;
using GoogleShopping.MerchantModule.Web.Providers;
using GoogleShopping.MerchantModule.Web.Services;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Framework.Web.Settings;
using Google.Apis.Auth.OAuth2;

namespace GoogleShopping.MerchantModule.Web.Controllers.Api
{
    [RoutePrefix("api/g")]
    public class GoogleShoppingController : ApiController
    {
        private const string _accessTokenPropertyName = "Google.Shopping.Credentials.AccessToken";
        private readonly IGoogleProductProvider _productProvider;
        private readonly IShopping _settingsManager;

        public GoogleShoppingController(IShopping settingsManager, IGoogleProductProvider productProvider)
        {
            _settingsManager = settingsManager;
            _productProvider = productProvider;
        }
        
        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("products/sync/{productId}")]
        public IHttpActionResult SyncProduct(string productId)
        {
            var service = Authorize();

            var products = _productProvider.GetProductUpdates(new[] { productId });
            products.ForEach(product => service.Products.Insert(product, _settingsManager.MerchantId).Execute());

            return Ok();
        }

        private ShoppingContentService Authorize()
        {
            var serviceAccountEmail = "39718569872-p1gucbblanda96o6nr9bbrjdekv8euba@developer.gserviceaccount.com";
            const string keyPath = @"D:\Virtoway\Projects\vc-community\PLATFORM\Modules\Merchant\GoogleShopping.Merchant.Web";
            const string keyName = "key.p12";

            var key = string.Format(@"{0}\{1}", keyPath, keyName);
            
            var certificate = new X509Certificate2(key, "notasecret", X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = new[] { ShoppingContentService.Scope.Content }
               }.FromCertificate(certificate));

            // Create the service.
            var service = new ShoppingContentService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Shopping Content Sample",

            });

            return service;
        }
    }
}

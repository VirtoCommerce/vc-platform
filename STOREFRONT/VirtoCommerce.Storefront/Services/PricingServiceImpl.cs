using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Converters;

namespace VirtoCommerce.Storefront.Services
{
    public class PricingServiceImpl : IPricingService
    {
        private readonly IPricingModuleApi _pricingApi;
        private readonly WorkContext _workContext;
        public PricingServiceImpl(WorkContext workContext, IPricingModuleApi pricingApi)
        {
            _pricingApi = pricingApi;
            _workContext = workContext;
        }

        #region IPricingService Members
        public async Task EvaluateProductPricesAsync(IEnumerable<Product> products)
        {
            //Evaluate products prices
            var pricesResponse = await _pricingApi.PricingModuleEvaluatePricesAsync(
                evalContextProductIds: products.Select(p => p.Id).ToList(),
                evalContextPricelistIds: _workContext.CurrentPriceListIds.ToList(),
                evalContextCatalogId: _workContext.CurrentStore.Catalog,
                evalContextCustomerId: _workContext.CurrentCustomer.Id,
                evalContextLanguage: _workContext.CurrentLanguage.CultureName,
                evalContextCertainDate: _workContext.StorefrontUtcNow,
                evalContextStoreId: _workContext.CurrentStore.Id);

            var alreadyDefinedProductsPriceGroups = pricesResponse.Select(x => x.ToWebModel(_workContext.AllCurrencies, _workContext.CurrentLanguage)).GroupBy(x=>x.ProductId);
            foreach (var product in products)
            {
                var productPricesGroup = alreadyDefinedProductsPriceGroups.FirstOrDefault(x => x.Key == product.Id);
                if(productPricesGroup != null)
                {
                    //Get first price for each currency
                    product.Prices = productPricesGroup.GroupBy(x => x.Currency).Select(x => x.FirstOrDefault()).Where(x=>x != null).ToList();
                }
                //Need add product price for all store currencies (even if not returned from api need make it by currency exchange convertation)
                foreach (var storeCurrency in _workContext.CurrentStore.Currencies)
                {
                    var price = product.Prices.FirstOrDefault(x => x.Currency == storeCurrency);
                    if (price == null)
                    {
                        price = new ProductPrice(storeCurrency);
                        if (product.Prices.Any())
                        {
                            price = product.Prices.First().ConvertTo(storeCurrency);
                        }
                        product.Prices.Add(price);
                    }
                }
                product.Currency = _workContext.CurrentCurrency;
                product.Price = product.Prices.FirstOrDefault(x => x.Currency.Equals(_workContext.CurrentCurrency));
            }

        }

        #endregion
    }
}
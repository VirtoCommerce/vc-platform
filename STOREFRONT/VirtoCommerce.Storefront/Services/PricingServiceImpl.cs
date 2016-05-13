using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Services
{
    public class PricingServiceImpl : IPricingService
    {
        private readonly IPricingModuleApi _pricingApi;
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly Func<WorkContext> _workContextFactory;
        public PricingServiceImpl(Func<WorkContext> workContextFactory, IPricingModuleApi pricingApi, ICommerceCoreModuleApi commerceApi)
        {
            _pricingApi = pricingApi;
            _commerceApi = commerceApi;
            _workContextFactory = workContextFactory;
        }

        #region IPricingService Members
        public async Task EvaluateProductPricesAsync(IEnumerable<Product> products)
        {
            var workContext = _workContextFactory();
            //Evaluate products prices
            var evalContext = products.ToServiceModel(workContext);

            var pricesResponse = await _pricingApi.PricingModuleEvaluatePricesAsync(evalContext);
            ApplyProductPricesInternal(products, pricesResponse);
            //Evaluate product taxes
            var taxEvalContext = workContext.ToTaxEvaluationContext(products);
            var taxRates = await _commerceApi.CommerceEvaluateTaxesAsync(workContext.CurrentStore.Id, taxEvalContext);
            ApplyProductTaxInternal(products, taxRates);
        }

        public void EvaluateProductPrices(IEnumerable<Product> products)
        {
            var workContext = _workContextFactory();
            //Evaluate products prices
            var evalContext = products.ToServiceModel(workContext);

            var pricesResponse = _pricingApi.PricingModuleEvaluatePrices(evalContext);
            ApplyProductPricesInternal(products, pricesResponse);

            //Evaluate product taxes
            var taxEvalContext = workContext.ToTaxEvaluationContext(products);
            var taxRates = _commerceApi.CommerceEvaluateTaxes(workContext.CurrentStore.Id, taxEvalContext);
            ApplyProductTaxInternal(products, taxRates);
        }

        #endregion
        private void ApplyProductTaxInternal(IEnumerable<Product> products, IEnumerable<VirtoCommerceDomainTaxModelTaxRate> taxes)
        {
            var workContext = _workContextFactory();
            var taxRates = taxes.Select(x => x.ToWebModel(workContext.CurrentCurrency));
            foreach(var product in products)
            {
                product.ApplyTaxRates(taxRates);
            }
        }

        private void ApplyProductPricesInternal(IEnumerable<Product> products, IEnumerable<VirtoCommercePricingModuleWebModelPrice> prices)
        {
            var workContext = _workContextFactory();

            foreach (var product in products)
            {
                var productPrices = prices.Where(x => x.ProductId == product.Id)
                                          .Select(x => x.ToWebModel(workContext.AllCurrencies, workContext.CurrentLanguage));
                product.ApplyPrices(productPrices, workContext.CurrentCurrency, workContext.CurrentStore.Currencies);
            }

        }
    }
}
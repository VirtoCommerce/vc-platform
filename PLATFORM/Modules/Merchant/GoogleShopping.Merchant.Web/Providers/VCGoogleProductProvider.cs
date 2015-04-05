using System.Collections.Generic;
using System.Linq;
using Google.Apis.ShoppingContent.v2.Data;
using GoogleShopping.MerchantModule.Web.Converters;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Foundation.Assets.Services;

namespace GoogleShopping.MerchantModule.Web.Providers
{
    public class VCGoogleProductProvider: IGoogleProductProvider
    {
        private readonly IItemService _itemService;
        private readonly IPricingService _pricingService;
        private readonly IAssetUrlResolver _assetUrlResolver;

        public VCGoogleProductProvider(IItemService itemService, IPricingService pricingService, IAssetUrlResolver assetUrlResolver)
        {
            _itemService = itemService;
            _pricingService = pricingService;
            _assetUrlResolver = assetUrlResolver;
        }

        public IEnumerable<Product> GetProductUpdates(IEnumerable<string> ids)
        {
            var retVal = new List<Product>();

            var items = _itemService.GetByIds(ids.ToArray(), ItemResponseGroup.ItemLarge);
            items.ForEach(product =>
            {
                var converted = product.ToGoogleModel(_assetUrlResolver);
                var prices = _pricingService.EvaluateProductPrices(new PriceEvaluationContext { ProductId = converted.Id });
                if (prices != null)
                {
                    converted.Price = prices.First().ToGoogleModel();
                    retVal.Add(converted);
                }
            });
            return retVal;
        }
    }
}
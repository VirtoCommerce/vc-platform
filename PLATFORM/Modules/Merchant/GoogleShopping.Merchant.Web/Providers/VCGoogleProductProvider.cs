using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Google.Apis.ShoppingContent.v2.Data;
using GoogleShopping.MerchantModule.Web.Converters;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;

namespace GoogleShopping.MerchantModule.Web.Providers
{
    public class VCGoogleProductProvider: IGoogleProductProvider
    {
        private readonly IItemService _itemService;
        private readonly IPricingService _pricingService;
        private readonly IBlobUrlResolver _assetUrlResolver;
        private readonly ICatalogSearchService _catalogSearchService;

		public VCGoogleProductProvider(IItemService itemService, IPricingService pricingService, IBlobUrlResolver assetUrlResolver, ICatalogSearchService catalogSearchService)
        {
            _itemService = itemService;
            _pricingService = pricingService;
            _assetUrlResolver = assetUrlResolver;
            _catalogSearchService = catalogSearchService;
        }

        public IEnumerable<Product> GetProductUpdates(IEnumerable<string> ids)
        {
            Collection<Product> retVal = null;

            var items = _itemService.GetByIds(ids.ToArray(), ItemResponseGroup.ItemLarge);
            items.ForEach(product =>
            {
                var converted = product.ToGoogleModel(_assetUrlResolver);
                var prices = _pricingService.EvaluateProductPrices(new PriceEvaluationContext { ProductIds = new string[] { converted.Id } });
                if (prices != null)
                {
                    converted.Price = prices.First(x => x.Currency == CurrencyCodes.USD).ToGoogleModel();
                    if (retVal == null)
                        retVal = new Collection<Product>();
                    retVal.Add(converted);
                }
            });
            return retVal;
        }

        public ProductsCustomBatchRequest GetProductsBatchRequest(IEnumerable<string> ids)
        {
            var retVal = new ProductsCustomBatchRequest();
            var products = GetProductUpdates(ids);
            retVal.Entries = new List<ProductsCustomBatchRequestEntry>();
            foreach(var prod in products.Select((value, index) => new {Value = value, Index = index}))
            {
                var val = prod.Value.ToBatchEntryModel();
                val.BatchId = prod.Index+1;
                retVal.Entries.Add(val); 
            };
            return retVal;
        }

        public ProductsCustomBatchRequest GetCatalogProductsBatchRequest(string catalogId, string categoryId = "")
        {
            var result = _catalogSearchService.Search(new SearchCriteria { CatalogId = catalogId, CategoryId = categoryId, ResponseGroup = ResponseGroup.WithProducts | ResponseGroup.WithVariations });
            var retVal = new ProductsCustomBatchRequest();
            foreach (var product in result.Products.Select((value, index) => new { Value = value, Index = index }))
            {
                var converted = product.Value.ToGoogleModel(_assetUrlResolver);
                var prices = _pricingService.EvaluateProductPrices(new PriceEvaluationContext { ProductIds = new string[] { converted.Id } });
                if (prices != null)
                {
                    converted.Price = prices.First(x => x.Currency == CurrencyCodes.USD).ToGoogleModel();
                    if (retVal.Entries == null)
                        retVal.Entries = new List<ProductsCustomBatchRequestEntry>();
                    var val = converted.ToBatchEntryModel();
                    val.BatchId = product.Index + 1;
                    retVal.Entries.Add(val);
                }
            };
            return retVal;
        }
    }
}
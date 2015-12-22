using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Amazon.MerchantModule.Web.Converters;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using AmazonMWSClientLib.Model.Feeds;

namespace Amazon.MerchantModule.Web.Providers
{
    public class VCAmazonProductProvider : IAmazonProductProvider
    {
        private readonly IItemService _itemService;
        private readonly IPricingService _pricingService;
        private readonly IBlobUrlResolver _assetUrlResolver;
        private readonly ICatalogSearchService _catalogSearchService;

		public VCAmazonProductProvider(IItemService itemService, IPricingService pricingService, IBlobUrlResolver assetUrlResolver, ICatalogSearchService catalogSearchService)
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
                var converted = product.ToAmazonModel(_assetUrlResolver);
                retVal.Add(converted);
            });
            return retVal;
        }

        public IEnumerable<Product> GetCatalogProductsBatchRequest(string catalogId, string categoryId = "")
        {
            var retVal = new List<Product>();
            var result = _catalogSearchService.Search(new SearchCriteria { CatalogId = catalogId, CategoryId = categoryId, ResponseGroup = SearchResponseGroup.WithProducts | SearchResponseGroup.WithVariations });
            
            foreach (var product in result.Products.Select((value, index) => new { Value = value, Index = index }))
            {
                var converted = product.Value.ToAmazonModel(_assetUrlResolver);
                
                retVal.Add(converted);
            };
            return retVal;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PriceEvaluationContextConverter
    {
        public static VirtoCommerceDomainPricingModelPriceEvaluationContext ToServiceModel(this IEnumerable<Product> products, WorkContext workContext)
        {
            if(products == null)
            {
                throw new ArgumentNullException("products");
            }
            //Evaluate products prices
            var retVal = new VirtoCommerceDomainPricingModelPriceEvaluationContext
            {
                ProductIds = products.Select(p => p.Id).ToList(),
                PricelistIds = workContext.CurrentPricelists.Select(p => p.Id).ToList(),
                CatalogId = workContext.CurrentStore.Catalog,
                CustomerId = workContext.CurrentCustomer.Id,
                Language = workContext.CurrentLanguage.CultureName,
                CertainDate = workContext.StorefrontUtcNow,
                StoreId = workContext.CurrentStore.Id
            };
            return retVal;

        }
    }
}
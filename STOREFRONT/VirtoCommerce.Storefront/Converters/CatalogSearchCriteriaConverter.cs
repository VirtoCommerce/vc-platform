using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CatalogSearchCriteriaConverter
    {
        public static VirtoCommerceDomainCatalogModelSearchCriteria ToServiceModel(this CatalogSearchCriteria criteria, WorkContext workContext)
        {
            var result = new VirtoCommerceDomainCatalogModelSearchCriteria
            {
                StoreId = workContext.CurrentStore.Id,
                Keyword = criteria.Keyword,
                ResponseGroup = criteria.ResponseGroup.ToString(),
                SearchInChildren = criteria.SearchInChildren,
                CategoryId = criteria.CategoryId,
                CatalogId = criteria.CatalogId,
                Currency = criteria.Currency == null ?  workContext.CurrentCurrency.Code : criteria.Currency.Code,
                HideDirectLinkedCategories = true,
                Terms = criteria.Terms.ToStrings(),
                PricelistIds = workContext.CurrentPricelists.Where(p => p.Currency == workContext.CurrentCurrency.Code).Select(p => p.Id).ToList(),
                Skip = criteria.Start,
                Take = criteria.PageSize,
                Sort = criteria.SortBy
            };
            return result;
        }

    }
}
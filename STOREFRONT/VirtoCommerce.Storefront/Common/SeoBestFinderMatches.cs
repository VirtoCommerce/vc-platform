using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Common
{
    /// <summary>
    /// Find best seo match based for passed store and language
    /// </summary>
    public static class SeoBestFinderMatches
    {
        public static VirtoCommerceDomainCommerceModelSeoInfo FindBestSeoMatch(this IEnumerable<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords, Language language, Store store)
        {
            VirtoCommerceDomainCommerceModelSeoInfo retVal = null;
            if (seoRecords != null)
            {
                retVal = seoRecords.Select(x =>
                {
                    var score = 0;
                    score += language.Equals(x.LanguageCode) ? 2 : 0;
                    score += store.Id.Equals(x.StoreId, StringComparison.OrdinalIgnoreCase) ? 3 : 0;
                    score += store.DefaultLanguage.Equals(x.LanguageCode) ? 1 : 0;
                    return new { SeoRecord = x, Score = score };
                }).OrderByDescending(x => x.Score).Select(x => x.SeoRecord).FirstOrDefault();
            }
            return retVal;
        }
    }
}
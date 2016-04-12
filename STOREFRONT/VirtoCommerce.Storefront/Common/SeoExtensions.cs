using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Common
{
    /// <summary>
    /// Find best seo match based for passed store and language
    /// </summary>
    public static class SeoExtensions
    {
        /// <summary>
        /// Returns SEO path if all outline items of the first outline have SEO keywords, otherwise returns default value.
        /// Path: GrandParentCategory/ParentCategory/ProductCategory/Product
        /// </summary>
        /// <param name="outlines"></param>
        /// <param name="store"></param>
        /// <param name="language"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSeoPath(this IEnumerable<VirtoCommerceDomainCatalogModelOutline> outlines, Store store, Language language, string defaultValue)
        {
            var result = defaultValue;

            if (outlines != null && store.SeoLinksType != SeoLinksType.None)
            {
                var outline = outlines.FirstOrDefault();

                if (outline != null)
                {
                    var pathSegments = new List<string>();

                    if (store.SeoLinksType == SeoLinksType.Long)
                    {
                        pathSegments.AddRange(outline.Items
                            .Where(i => i.SeoObjectType != "Catalog")
                            .Select(i => GetBestMatchedSeoKeyword(i.SeoInfos, store, language)));
                    }
                    else
                    {
                        var lastItem = outline.Items.LastOrDefault();
                        if (lastItem != null)
                        {
                            pathSegments.Add(GetBestMatchedSeoKeyword(lastItem.SeoInfos, store, language));
                        }
                    }


                    if (pathSegments.All(s => s != null))
                    {
                        result = string.Join("/", pathSegments);
                    }
                }
            }

            return result;
        }


        public static VirtoCommerceDomainCommerceModelSeoInfo GetBestMatchedSeoInfo(this IEnumerable<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords, Store store, Language language)
        {
            VirtoCommerceDomainCommerceModelSeoInfo result = null;

            if (seoRecords != null)
            {
                result = seoRecords
                    .Select(s =>
                    {
                        var score = 0;
                        score += store.Id.Equals(s.StoreId, StringComparison.OrdinalIgnoreCase) ? 4 : 0;
                        score += language.Equals(s.LanguageCode) ? 2 : 0;
                        score += store.DefaultLanguage.Equals(s.LanguageCode) ? 1 : 0;
                        return new { SeoRecord = s, Score = score };
                    })
                    .OrderByDescending(x => x.Score)
                    .Select(x => x.SeoRecord)
                    .FirstOrDefault();
            }

            return result;
        }


        private static string GetBestMatchedSeoKeyword(IEnumerable<VirtoCommerceDomainCommerceModelSeoInfo> seoRecords, Store store, Language language)
        {
            string result = null;

            if (seoRecords != null)
            {
                // Select best matched SEO by StoreId and Language
                var bestMatchedSeo = seoRecords.GetBestMatchedSeoInfo(store, language);
                if (bestMatchedSeo != null)
                {
                    result = bestMatchedSeo.SemanticUrl;
                }
            }

            return result;
        }
    }
}

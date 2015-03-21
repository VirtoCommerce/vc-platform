#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#endregion

namespace VirtoCommerce.Web.Models.Extensions
{
    public static class SiteContextExtensions
    {
        #region Public Methods and Operators
        /// <summary>
        ///     Gets the store. First treats slug as storeId then as keyword.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="slug"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Shop GetShopBySlug(this SiteContext context, string slug, string language = null)
        {
            var allShops = context.Shops;

            if (!allShops.Any())
            {
                return null;
            }

            var store = allShops.FirstOrDefault(x => x.StoreId.Equals(slug, StringComparison.OrdinalIgnoreCase));

            if (store == null)
            {
                var keyword = allShops.SelectMany(x => x.Keywords).Where(x => x.Keyword.Equals(slug, StringComparison.InvariantCultureIgnoreCase)).SeoKeyword(language);

                if (keyword != null)
                {
                    store = allShops.FirstOrDefault(x => x.StoreId.Equals(keyword.Keyword, StringComparison.InvariantCultureIgnoreCase));
                }

                /*
                foreach (var shop in allShops.Where(shop => shop.Keywords.SeoKeyword(language) != null))
                {
                    return shop;
                }
                 * */
            }
            return store;
        }

        /// <summary>
        ///     Get correct seo keyword by language from list of given keywords
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static SeoKeyword SeoKeyword(this IEnumerable<SeoKeyword> keywords, string language = null)
        {
            if (keywords == null || !keywords.Any())
            {
                return null;
            }

            var langInfo = language != null ? language.TryGetCultureInfo() : Thread.CurrentThread.CurrentUICulture;
            language = langInfo != null ? langInfo.Name : language;

            //Filter keywords with valid language
            keywords = keywords.Where(x => x.Language.TryGetCultureInfo() != null).ToArray();

            if (keywords.Any())
            {
                var seoKeyword =
                    keywords.FirstOrDefault(
                        x => x.Language.Equals(language, StringComparison.InvariantCultureIgnoreCase));

                if (seoKeyword != null)
                {
                    return seoKeyword;
                }

                //Default language failover scenario
                var store = SiteContext.Current.Shop;

                //if (store == null) throw new NullReferenceException("store is not initialized");
                //Current store can be null when called from StoreHttpModule and store is not yet initialzed
                /*
                if (store == null && keywords[0].KeywordType == SeoUrlKeywordTypes.Store)
                {
                    store = StoreHelper.StoreClient.GetStore(keywords[0].KeywordValue);
                }
                 * */

                if (store != null && !store.DefaultLanguage.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    return
                        keywords.FirstOrDefault(
                            x => x.Language.Equals(store.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return null;
        }
        #endregion
    }
}
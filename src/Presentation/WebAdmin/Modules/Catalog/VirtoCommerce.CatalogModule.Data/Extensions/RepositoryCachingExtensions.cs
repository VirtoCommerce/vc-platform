using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CatalogModule.Data.Extensions
{
    public static class RepositoryCachingExtensions
    {
        #region Constants
        public const string ItemsCacheKey = "I:{0}:rg:{1}";
        public const string CatalogCacheKey = "C:{0}";
        #endregion

        public static IEnumerable<Item> GetItemsCached(this IFoundationCatalogRepository repository, ICacheRepository cache, ISettingsManager settingsManager, string[] itemIds, ItemResponseGroup respGroup)
        {
            var cacheHelper = new CacheHelper(cache);
            var itemTimeout = 30;//settingsManager.GetValue("Catalogs.Caching.ItemTimeout", 30);
            return cacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix, string.Format(ItemsCacheKey, CacheHelper.CreateCacheKey(itemIds), respGroup.GetHashCode())),
                () => repository.GetItemByIds(itemIds, respGroup).ToArray(),
                new TimeSpan(0, 0, itemTimeout));
        }

        public static CatalogBase GetCatalogCached(this IFoundationCatalogRepository repository, ICacheRepository cache, ISettingsManager settingsManager, string catalogId)
        {
            var cacheHelper = new CacheHelper(cache);
            var catalogTimeout = 30;//settingsManager.GetValue("Catalogs.Caching.CatalogTimeout", 60);
            return cacheHelper.Get(
                CacheHelper.CreateCacheKey(Constants.CatalogCachePrefix, string.Format(CatalogCacheKey, catalogId)),
                () => repository.GetCatalogById(catalogId),
                new TimeSpan(0, 0, catalogTimeout));
        }
    }
}

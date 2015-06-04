using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.Caching
{
    public interface IContentStore
    {
        // Fast lookup to determine if any representations exist for the method and URI
        // Every HTTP request pays this cost regardless of hit or not

        // Retreive actual content based on variant selection key

        #region Public Methods and Operators

        Task<CacheContent> GetContentAsync(CacheEntry entry, string secondaryKey);
        Task<CacheEntry> GetEntryAsync(PrimaryCacheKey cacheKey);

        Task UpdateEntryAsync(CacheContent content);

        #endregion
    }
}

using System.Web.Mvc;

namespace VirtoCommerce.ApiWebClient.Caching.Interfaces
{
    public interface IKeyGenerator
    {
        /// <summary>
        /// Implementations should generate a key given the <see cref="context"/> and <see cref="cacheSettings"/>.
        /// </summary>
        /// <param name="context">The controller context.</param>
        /// <param name="cacheSettings">The cache settings.</param>
        /// <returns>A string that can be used as an output cache key</returns>
        string GenerateKey(ControllerContext context, CacheSettings cacheSettings);
    }
}

using System.Web.UI;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Caching
{
    public class CacheSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the cache is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cache is enabled otherwise, <c>false</c>.
        /// </value>
        public bool IsCachingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the cache duration.
        /// </summary>
        /// <value>
        /// The cache duration.
        /// </value>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the VaryByParam cache parameter.
        /// </summary>
        /// <value>
        /// The VaryByParam cache parameter.
        /// </value>
        public string VaryByParam { get; set; }

        /// <summary>
        /// Gets or sets the VaryByCustom cache parameter.
        /// </summary>
        /// <value>
        /// The VaryByCustom cache parameter.
        /// </value>
        public string VaryByCustom { get; set; }

        /// <summary>
        /// Gets or sets the output cache location.
        /// </summary>
        /// <value>
        /// The output cache location.
        /// </value>
        public OutputCacheLocation Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether store or not the result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if no store; otherwise, <c>false</c>.
        /// </value>
        public bool NoStore { get; set; }

        /// <summary>
        /// Gets or sets the output cache options.
        /// </summary>
        /// <value>
        /// The output cache options.
        /// </value>
        public OutputCacheOptions Options { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether cache is for anonymous users only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cache is for anonymous users only otherwise, <c>false</c>.
        /// </value>
        public bool AnonymousOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the server caching is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the server caching enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsServerCachingEnabled
        {
            get
            {
                return IsCachingEnabled && Duration > 0 && (Location == OutputCacheLocation.Any ||
                                                            Location == OutputCacheLocation.Server ||
                                                            Location == OutputCacheLocation.ServerAndClient)
                                                            && (!AnonymousOnly || AnonymousOnly && !StoreHelper.CustomerSession.IsRegistered);
            }
        }

    }
}

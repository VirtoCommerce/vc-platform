using System;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Core.Configuration.DynamicContent;
using VirtoCommerce.Web.Core.DataContracts;
using VirtoCommerce.Web.Core.DataContracts.Contents;

namespace VirtoCommerce.ApiWebClient.Clients
{
    using VirtoCommerce.ApiClient.Caching;
    using VirtoCommerce.ApiClient.Session;

    using CacheHelper = VirtoCommerce.ApiWebClient.Caching.CacheHelper;

    public class DynamicContentClient
    {
        #region Cache Constants
        public const string DynamicContentCacheKey = "M:DC:S{0}:{1}:{2}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
	    private readonly ICacheRepository _cacheRepository;
        private readonly ICustomerSessionService _customerSession;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicContentClient" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="customerSession">The customer session.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public DynamicContentClient(ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
		    _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _isEnabled = DynamicContentConfiguration.Instance.Cache.IsEnabled;
        }

        public ContentClient GetClient(string lang)
        {
            return ClientContext.Clients.CreateDefaultContentClient(lang);

        }

        /// <summary>
        /// Gets the content of the dynamic.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <param name="language">The language.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="placeName">Name of the place.</param>
        /// <returns></returns>
        public async Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(TagQuery tags, string language, DateTime timestamp, params string[] placeName)
        {
            var client = GetClient(language);

            return await Helper.GetAsync(
                CacheHelper.CreateCacheKey(Constants.DynamicContentCachePrefix, string.Format(DynamicContentCacheKey, placeName, timestamp, tags.GetCacheKey())),
                () => client.GetDynamicContentAsync(placeName, tags),
                DynamicContentConfiguration.Instance.Cache.DynamicContentTimeout, _isEnabled);
        }

		CacheHelper _cacheHelper;
        /// <summary>
        /// Gets the helper.
        /// </summary>
        /// <value>
        /// The helper.
        /// </value>
		public CacheHelper Helper
		{
			get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
		}
    }

}

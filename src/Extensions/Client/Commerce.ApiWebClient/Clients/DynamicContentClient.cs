using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;
using VirtoCommerce.ApiWebClient.Configuration.DynamicContent;
using VirtoCommerce.ApiWebClient.Customer.Services;

namespace VirtoCommerce.ApiWebClient.Clients
{
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

        public ContentClient ContentClient
        {
            get
            {
                //TODO: get correct language
                return ClientContext.Clients.CreateContentClient(string.Format(DynamicContentConfiguration.Instance.Connection.DataServiceUri, "en-us"));
            }
        }

        /// <summary>
        /// Gets the content of the dynamic.
        /// </summary>
        /// <param name="placeName">Name of the place.</param>
        /// <returns></returns>
        public async Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(params string[] placeName)
        {
            var session = _customerSession.CustomerSession;
            var tags = session.GetCustomerTagSet();

            return await Helper.GetAsync(
                CacheHelper.CreateCacheKey(Constants.DynamicContentCachePrefix, string.Format(DynamicContentCacheKey, placeName, session.CurrentDateTime, tags.GetCacheKey())),
                () => ContentClient.GetDynamicContentAsync(placeName, tags),
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

using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Services;

namespace VirtoCommerce.Client
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
        private readonly IDynamicContentService _service;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicContentClient" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="customerSession">The customer session.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public DynamicContentClient(IDynamicContentService service, ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
		    _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _service = service;
            _isEnabled = DynamicContentConfiguration.Instance.Cache.IsEnabled;
        }

        /// <summary>
        /// Gets the content of the dynamic.
        /// </summary>
        /// <param name="placeName">Name of the place.</param>
        /// <returns></returns>
        public DynamicContentItem[] GetDynamicContent(string placeName)
        {
            var session = _customerSession.CustomerSession;
            var tags = session.GetCustomerTagSet();

            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.DynamicContentCachePrefix, string.Format(DynamicContentCacheKey, placeName, session.CurrentDateTime, tags.GetCacheKey())),
                () => _service.GetItems(placeName, session.CurrentDateTime, tags),
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

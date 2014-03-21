using System.Collections.Generic;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Services;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Tagging;
namespace VirtoCommerce.Client
{
    public class DisplayTemplateClient
    {
        #region Cache Constants
        public const string DisplayTemplateCacheKey = "M:DT:{0}:{1}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICustomerSessionService _customerSession;
        private readonly IDisplayTemplatesService _service;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayTemplateClient" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="customerSession">The customer session.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public DisplayTemplateClient(IDisplayTemplatesService service, ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _service = service;
            _isEnabled = AppConfigConfiguration.Instance.Cache.IsEnabled;
        }

        /// <summary>
        /// Gets the display template.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="tags">The tags.</param>
        /// <returns></returns>
        public string GetDisplayTemplate(TargetTypes target, TagSet tags)
        {
            var session = _customerSession.CustomerSession;
            var tagsLocal = tags ?? session.GetCustomerTagSet();

            return Helper.Get(
                CacheHelper.CreateCacheKey(Constants.DisplayTemplateCachePrefix, string.Format(DisplayTemplateCacheKey, target, tagsLocal.GetCacheKey())),
                () => _service.GetTemplate(target, tags),
                AppConfigConfiguration.Instance.Cache.DisplayTemplateMappingsTimeout,
                _isEnabled);
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

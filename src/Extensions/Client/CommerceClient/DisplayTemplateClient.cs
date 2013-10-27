using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
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
        private bool IsEnabled = false;
        private IAppConfigRepository _contentRepository;
        private ICacheRepository _cacheRepository;
        private ICustomerSessionService _customerSession;
        private IDisplayTemplatesService _service;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayTemplateClient" /> class.
        /// </summary>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="service">The service.</param>
        /// <param name="customerSession">The customer session.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public DisplayTemplateClient(IAppConfigRepository contentRepository, IDisplayTemplatesService service, ICustomerSessionService customerSession, ICacheRepository cacheRepository)
        {
            _contentRepository = contentRepository;
            _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _service = service;
            IsEnabled = AppConfigConfiguration.Instance.Cache.IsEnabled;
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
                string.Format(DisplayTemplateCacheKey, target.ToString(), tagsLocal.GetCacheKey()),
                () => _service.GetTemplate(target, tags),
                AppConfigConfiguration.Instance.Cache.DisplayTemplateMappingsTimeout,
                IsEnabled);
        }

        CacheHelper _CacheHelper = null;
        /// <summary>
        /// Gets the helper.
        /// </summary>
        /// <value>
        /// The helper.
        /// </value>
        public CacheHelper Helper
        {
            get
            {
                if (_CacheHelper == null)
                    _CacheHelper = new CacheHelper(_cacheRepository);

                return _CacheHelper;
            }
        }
    }

}

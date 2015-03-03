using FunctionalTests.TestHelpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;

namespace FunctionalTests.Marketing.Helpers
{
    public abstract class MarketingTestBase : TestBase
    {
        private UnityContainer GetLocalContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<ICacheRepository, HttpCacheRepository>();

            #region Marketing
            container.RegisterType<IMarketingRepository, EFMarketingRepository>();
            container.RegisterType<IMarketingEntityFactory, MarketingEntityFactory>();
            container.RegisterType<IPromotionUsageProvider, PromotionUsageProvider>();
            container.RegisterType<IPromotionEntryPopulate, PromotionEntryPopulate>();
            #endregion

            #region DynamicContent
            container.RegisterType<IDynamicContentService, DynamicContentService>();
            container.RegisterType<IDynamicContentRepository, EFDynamicContentRepository>();
            container.RegisterType<IDynamicContentEvaluator, DynamicContentEvaluator>();
            #endregion

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
            return container;
        }

        UnityContainer _container = null;
        protected IServiceLocator Locator
        {
            get
            {
                if (_container == null)
                {
                    _container = GetLocalContainer();
                    ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(_container));
                }

                return ServiceLocator.Current;
            }
        }

        protected IDynamicContentService DynamicContentService
        {
            get
            {
                return Locator.GetInstance<IDynamicContentService>();
            }
        }
    }
}

using linq = System.Linq.Expressions;
using System;
using System.IO;
using System.Linq;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Client.Globalization.Repository;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Localization
{
    public class LocalizationModule : IModule
    {
        private readonly IUnityContainer _container;

        public LocalizationModule(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
        }

        #endregion

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IAppConfigEntityFactory, AppConfigEntityFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAppConfigRepository, DSAppConfigClient>();
            var loginViewModel = _container.Resolve<ILoginViewModel>();
            var baseUrlHash = loginViewModel.CurrentUser.BaseUrl.ToLower().GetHashCode().ToString();

            var repositoryFactory = _container.Resolve<IRepositoryFactory<IAppConfigRepository>>();
            var localElements = new XmlElementRepository(Path.Combine(Path.GetTempPath(), "VirtoCommerceCMLocalization", baseUrlHash));
            var cachedElements = new CacheElementRepository(localElements);
            var _elementRepository = new CachedDatabaseElementRepository(repositoryFactory, cachedElements, GetDefaultFilter());
            _container.RegisterInstance<IElementRepository>(_elementRepository);

            // check cache date and update if needed
            var repository = _container.Resolve<IAppConfigRepository>();
            var lastItem = repository.Localizations.OrderByDescending(x => x.LastModified).Take(1).FirstOrDefault();
            DateTime? dbDate = lastItem == null ? null : lastItem.LastModified;

            var cacheDate = _elementRepository.GetStatusDate();
            if (dbDate.HasValue && dbDate > cacheDate)
            {
                _elementRepository.Clear();

                // force Elements re-caching
                _elementRepository.Elements();

                _elementRepository.SetStatusDate(dbDate.Value);
            }
        }

        private linq.Expression<Func<Foundation.AppConfig.Model.Localization, bool>> GetDefaultFilter()
	    {
            // the expression is: (x => x.Category != "")
            var parameter = linq.Expression.Parameter(typeof(Foundation.AppConfig.Model.Localization), "x");
            linq.Expression condition = linq.Expression.NotEqual(
                    linq.Expression.Property(parameter, "Category"),
                    linq.Expression.Constant(""));

            return linq.Expression.Lambda<Func<Foundation.AppConfig.Model.Localization, bool>>(condition, parameter);
	    }
    }
}

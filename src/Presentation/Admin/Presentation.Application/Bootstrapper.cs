using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;

namespace VirtoCommerce.ManagementClient
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureContainer()
        {
            //Enable validation for StorageEntity
            Foundation.Configuration.EnableEntityValidation = true;

            Container.RegisterType<INavigationManager, NavigationManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<TileManager, TileManager>(new ContainerControlledLifetimeManager());
            
			//Repository factory
			Container.RegisterType(typeof(IRepositoryFactory<>), typeof(UnityRepositoryFactory<>), new ContainerControlledLifetimeManager());

			//ViewModels factory
			Container.RegisterType(typeof(IViewModelsFactory<>), typeof(UnityViewModelsFactory<>), new ContainerControlledLifetimeManager());

            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
            
            ((Window)Shell).Show();
        }
    }
}

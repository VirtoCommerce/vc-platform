using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ConfigurationUtility.Application.Views;

namespace VirtoCommerce.ConfigurationUtility.Application
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        //// Calling ConfigureModuleCatalog

        protected override void ConfigureContainer()
        {
            Container.RegisterType<NavigationManager, NavigationManager>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
        }

        //// Calling ConfigureServiceLocator

        //// Calling ConfigureRegionAdapterMappings

        //// Calling ConfigureDefaultRegionBehaviors

        //// Calling RegisterFrameworkExceptionTypes

        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            ((Window)Shell).Show();
        }

        //// Calling InitializeModules

        //// Calling Run
    }
}
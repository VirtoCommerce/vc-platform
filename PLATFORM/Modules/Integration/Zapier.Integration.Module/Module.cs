using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using Zapier.IntegrationModule.Web.Controllers.Api;
using Zapier.IntegrationModule.Web.Providers.Implementations;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            _container.RegisterType<IContactsProvider, ContactsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersProvider, OrdersProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<PollingController>();
        }
    }
}
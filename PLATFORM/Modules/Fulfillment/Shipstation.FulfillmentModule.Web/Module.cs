using Microsoft.Practices.Unity;
using Shipstation.FulfillmentModule.Web.Controllers;
using VirtoCommerce.Platform.Core.Modularity;

namespace Shipstation.FulfillmentModule.Web
{
    public class Module: ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members
        
        public override void Initialize()
        {
            _container.RegisterType<ShipstationController>();
        }
        
        #endregion
    }
}
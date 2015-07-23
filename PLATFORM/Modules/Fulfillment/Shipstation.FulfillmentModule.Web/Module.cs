using Microsoft.Practices.Unity;
using Shipstation.FulfillmentModule.Web.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Shipstation.FulfillmentModule.Web
{
    public class Module: ModuleBase
    {
        private const string _usernamePropertyName = "Shipstation.Fulfillment.Credentials.Username";
        private const string _passwordPropertyName = "Shipstation.Fulfillment.Credentials.Password";

        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members
        
        public override void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            var shipStation = new ShipstationSettings(_usernamePropertyName, _passwordPropertyName, settingsManager);
            _container.RegisterInstance<IFulfillmentSettings>(shipStation);
        }
        
        #endregion
    }
}
using Amazon.MerchantModule.Web.Controllers.Api;
using Amazon.MerchantModule.Web.Helpers.Implementations;
using Amazon.MerchantModule.Web.Helpers.Interfaces;
using Amazon.MerchantModule.Web.Services;
using MarketplaceWebService;
using MarketplaceWebService.Mock;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;

namespace Amazon.MerchantModule.Web
{
    public class Module : ModuleBase
    {
        private const string _merchantIdPropertyName = "Amazon.MerchantModule.MerchantId";
        private const string _marketplaceIdPropertyName = "Amazon.MerchantModule.MarketplaceId";
        private const string _awsAccessKeyIdPropertyName = "Amazon.MerchantModule.MwsAccessKeyId";
        private const string _awsSecretAccessKeyPropertyName = "Amazon.MerchantModule.MwsSecretAccessKey";
        private const string _serviceUrlPropertyName = "Amazon.MerchantModule.ServiceUrl";

        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            

            var amazonManager = new AmazonManagerSettings(
                settingsManager,
                _merchantIdPropertyName,
                _serviceUrlPropertyName,
                _marketplaceIdPropertyName,
                _awsAccessKeyIdPropertyName,
                _awsSecretAccessKeyPropertyName);

            var feedConfig = new MarketplaceWebServiceConfig { ServiceURL = amazonManager.ServiceURL };

            //test mock registration
            _container.RegisterInstance<IMarketplaceWebServiceClient>(new MockMarketplaceWebServiceClient());

            //production registration
            //_container.RegisterInstance<IMarketplaceWebServiceClient>(new MarketplaceWebServiceClient(amazonManager.AwsAccessKeyId, amazonManager.AwsSecretAccessKey, "VirtoCommerce", "1.01", feedConfig));

            _container.RegisterInstance<IDateTimeProvider>(new DefaultDateTimeProvider());
            _container.RegisterInstance<IAmazonSettings>(amazonManager);

            _container.RegisterType<AmazonMerchantController>();
        }

        #endregion
    }
}
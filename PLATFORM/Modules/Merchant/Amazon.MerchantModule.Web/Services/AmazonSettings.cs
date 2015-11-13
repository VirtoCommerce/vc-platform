using VirtoCommerce.Platform.Core.Settings;

namespace Amazon.MerchantModule.Web.Services
{
    public class AmazonManagerSettings : IAmazonSettings
    {
        private readonly string _merchantIdPropertyName;
        private readonly string _serviceUrlPropertyName;
        private readonly string _marketplaceIdPropertyName;
        private readonly string _awsAccessKeyIdPropertyName;
        private readonly string _awsSecretAccessKeyPropertyName;
        
        private readonly ISettingsManager _settingsManager;

        public AmazonManagerSettings(
            ISettingsManager settingsManager, 
            string merchantIdPropertyName,
            string serviceUrlPropertyName,
            string marketplaceIdPropertyName,
            string awsAccessKeyIdPropertyName,
            string awsSecretAccessKeyPropertyName)
        {
            _merchantIdPropertyName = merchantIdPropertyName;
            _serviceUrlPropertyName = serviceUrlPropertyName;
            _marketplaceIdPropertyName = marketplaceIdPropertyName;
            _awsAccessKeyIdPropertyName = awsAccessKeyIdPropertyName;
            _awsSecretAccessKeyPropertyName = awsSecretAccessKeyPropertyName;

            _settingsManager = settingsManager;
        }

        public string MerchantId
        {
            get
            {
                var retVal = _settingsManager.GetValue<string>(_merchantIdPropertyName, string.Empty);
                return retVal;
            }
        }

        public string ServiceURL
        {
            get
            {
                var retVal = _settingsManager.GetValue<string>(_serviceUrlPropertyName, "http://mws.amazonservices.com/");
                return retVal;
            }
        }

        public string MarketplaceId
        {
            get
            {
                var retVal = _settingsManager.GetValue<string>(_marketplaceIdPropertyName, string.Empty);
                return retVal;
            }
        }

        public string AwsAccessKeyId
        {
            get
            {
                var retVal = _settingsManager.GetValue<string>(_awsAccessKeyIdPropertyName, string.Empty);
                return retVal;
            }
        }

        public string AwsSecretAccessKey
        {
            get
            {
                var retVal = _settingsManager.GetValue<string>(_awsSecretAccessKeyPropertyName, string.Empty);
                return retVal;
            }
        }        
    }
}
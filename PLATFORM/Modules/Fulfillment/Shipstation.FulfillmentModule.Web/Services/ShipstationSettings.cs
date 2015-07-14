using VirtoCommerce.Platform.Core.Settings;

namespace Shipstation.FulfillmentModule.Web.Services
{
    public class ShipstationSettings: IFulfillmentSettings
    {
        private readonly string _usernamePropertyName;
            private readonly string _passwordPropertyName;
            private readonly string _serviceUrlPropertyName;

            private readonly ISettingsManager _settingsManager;

            public ShipstationSettings(string usernamePropertyName, string passwordPropertyName, string serviceUrlPropertyName, ISettingsManager settingsManager)
            {
                _usernamePropertyName = usernamePropertyName;
                _passwordPropertyName = passwordPropertyName;
                _serviceUrlPropertyName = serviceUrlPropertyName;
                _settingsManager = settingsManager;
            }

            public string Username
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_usernamePropertyName, string.Empty);
                    return retVal;
                }
            }

            public string Password
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_passwordPropertyName, string.Empty);
                    return retVal;
                }
            }
            
            public string ServiceUrl
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_serviceUrlPropertyName, string.Empty);
                    return retVal;
                }
            }
    }
}
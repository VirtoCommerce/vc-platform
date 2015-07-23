using VirtoCommerce.Platform.Core.Settings;

namespace Shipstation.FulfillmentModule.Web.Services
{
    public class ShipstationSettings : IFulfillmentSettings
    {
        private readonly string _usernamePropertyName;
        private readonly string _passwordPropertyName;

        private readonly ISettingsManager _settingsManager;

        public ShipstationSettings(
            string usernamePropertyName,
            string passwordPropertyName,
            ISettingsManager settingsManager)
        {
            _usernamePropertyName = usernamePropertyName;
            _passwordPropertyName = passwordPropertyName;
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
    }
}
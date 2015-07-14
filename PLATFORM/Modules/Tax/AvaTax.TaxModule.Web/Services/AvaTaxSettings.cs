using AvaTax.TaxModule.Web.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web.Services
{
    
        public class AvaTaxSettings : ITaxSettings
        {
            private readonly string _usernamePropertyName;
            private readonly string _passwordPropertyName;
            private readonly string _serviceUrlPropertyName;
            private readonly string _companyCodePropertyName;
            private readonly string _isEnabledPropertyName;
            private readonly string _isValidateAddressPropertyName;

            private readonly ISettingsManager _settingsManager;

            public AvaTaxSettings(string usernamePropertyName, string passwordPropertyName, string serviceUrlPropertyName, string companyCodePropertyName, string isEnabledPropertyName, string isValidateAddressPropertyName, ISettingsManager settingsManager)
            {
                _usernamePropertyName = usernamePropertyName;
                _passwordPropertyName = passwordPropertyName;
                _serviceUrlPropertyName = serviceUrlPropertyName;
                _companyCodePropertyName = companyCodePropertyName;
                _isEnabledPropertyName = isEnabledPropertyName;
                _isValidateAddressPropertyName = isValidateAddressPropertyName;
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

            public string CompanyCode
            {
                get 
                {
                    var retVal = _settingsManager.GetValue(_companyCodePropertyName, string.Empty);
                    return retVal;
                }
            }

            public bool IsEnabled
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_isEnabledPropertyName, true);
                    return retVal;
                }
            }

            public bool IsValidateAddress
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_isValidateAddressPropertyName, true);
                    return retVal;
                }
            }
        }
}
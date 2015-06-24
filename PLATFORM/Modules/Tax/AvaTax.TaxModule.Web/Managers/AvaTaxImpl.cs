using AvaTax.TaxModule.Web.Services;
using VirtoCommerce.Platform.Core.Settings;

namespace AvaTax.TaxModule.Web.Managers
{
    
        public class AvaTaxImpl : ITax
        {
            private readonly string _username;
            private readonly string _password;
            private readonly string _serviceUrl;
            private readonly string _companyCode;
            private readonly string _isEnabled;

            private readonly ISettingsManager _settingsManager;

            public AvaTaxImpl(string username, string password, string serviceUrl, string companyCode, string isEnabled, ISettingsManager settingsManager)
            {
                _username = username;
                _password = password;
                _serviceUrl = serviceUrl;
                _companyCode = companyCode;
                _isEnabled = isEnabled;
                _settingsManager = settingsManager;
            }

            public string Username
            {

                get
                {
                    var retVal = _settingsManager.GetValue(_username, string.Empty);
                    return retVal;
                }
            }

            public string Password
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_password, string.Empty);
                    return retVal;
                }
            }
            
            public string ServiceUrl
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_serviceUrl, string.Empty);
                    return retVal;
                }
            }

            public string CompanyCode
            {
                get 
                {
                    var retVal = _settingsManager.GetValue(_companyCode, string.Empty);
                    return retVal;
                }
            }

            public bool IsEnabled
            {
                get
                {
                    var retVal = _settingsManager.GetValue(_isEnabled, true);
                    return retVal;
                }
            }
        }
}
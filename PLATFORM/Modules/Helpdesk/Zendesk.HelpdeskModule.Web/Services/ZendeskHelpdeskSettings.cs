using VirtoCommerce.Platform.Core.Settings;
using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web.Services
{
    public class ZendeskHelpdeskSettings : IHelpdeskSettings
    {
        private readonly ISettingsManager _settingsManager;
        private readonly string _apiAccessTokenPropertyName;
        private readonly string _customerEmailPropertyName;        
        private readonly string _subdomainPropertyName;

        public ZendeskHelpdeskSettings(ISettingsManager settingsManager, string apiAccessTokenPropertyName, string subdomainPropertyName, string customerEmailPropertyName)
        {
            _apiAccessTokenPropertyName = apiAccessTokenPropertyName;            
            _subdomainPropertyName = subdomainPropertyName;
            _customerEmailPropertyName = customerEmailPropertyName;
            _settingsManager = settingsManager;
        }

        public string AccessToken
        {
            get
            {
                var retVal = _settingsManager.GetValue(_apiAccessTokenPropertyName, string.Empty);
                return retVal;
            }
            set
            {
                _settingsManager.SetValue(_apiAccessTokenPropertyName, value);
            }
        }

        public string CustomerEmail
        {
            get
            {
                var retVal = _settingsManager.GetValue(_customerEmailPropertyName, string.Empty);
                return retVal;
            }
        }

        public string Subdomain
        {
            get
            {
                var retVal = _settingsManager.GetValue(_subdomainPropertyName, string.Empty);
                return retVal;
            }
        }
    }
}
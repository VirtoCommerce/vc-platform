using VirtoCommerce.Platform.Core.Settings;
using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web.Services
{
    public class ZendeskHelpdeskSettings : IHelpdeskSettings
    {
        private readonly ISettingsManager _settingsManager;
        private readonly string _apiAccessTokenPropertyName;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;
        private readonly string _subdomainPropertyName;

        public ZendeskHelpdeskSettings(ISettingsManager settingsManager, string apiAccessTokenPropertyName, string subdomainPropertyName, string code, string description, string logoUrl)
        {
            _apiAccessTokenPropertyName = apiAccessTokenPropertyName;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;
            _subdomainPropertyName = subdomainPropertyName;

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

        public string Code
        {
            get { return _code; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string LogoUrl
        {
            get { return _logoUrl; }
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
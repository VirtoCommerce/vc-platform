using VirtoCommerce.Platform.Core.Settings;

namespace MailChimp.MailingModule.Web.Services
{
    public class MailChimpMailingSettings : IMailingSettings
    {
        private readonly ISettingsManager _settingsManager;
        private readonly string _apiAccessTokenPropertyName;
        private readonly string _apiDataCenterPropertyName;
        private readonly string _mailChimpSubscribeListPropertyName;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;

        public MailChimpMailingSettings(ISettingsManager settingsManager, string apiAccessTokenPropertyName, string apiDataCenterPropertyName, string mailChimpSubscribeListPropertyName, string code, string description, string logoUrl)
        {
            _apiAccessTokenPropertyName = apiAccessTokenPropertyName;
            _apiDataCenterPropertyName = apiDataCenterPropertyName;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;
            _mailChimpSubscribeListPropertyName = mailChimpSubscribeListPropertyName;
            _settingsManager = settingsManager;
        }

        public string AccessToken
        {
            get
            {
                var retVal = _settingsManager.GetValue(_apiAccessTokenPropertyName, string.Empty);
                return retVal;
            }
        }

        public string DataCenter
        {
            get
            {
                var retVal = _settingsManager.GetValue(_apiDataCenterPropertyName, string.Empty);
                return retVal;
            }
        }

        public string SubscribersListId
        {
            get
            {
                var retVal = _settingsManager.GetValue(_mailChimpSubscribeListPropertyName, string.Empty);
                return retVal;
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
    }
}
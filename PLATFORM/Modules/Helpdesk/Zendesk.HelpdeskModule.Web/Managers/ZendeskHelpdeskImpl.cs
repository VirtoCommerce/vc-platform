using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web.Managers
{
    public class ZendeskHelpdeskImpl: IHelpdesk
   {
        private readonly string _apiAccessToken;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;

        public ZendeskHelpdeskImpl(string apiAccessToken, string code, string description, string logoUrl)
        {
            _apiAccessToken = apiAccessToken;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;
        }

        public string AccessToken
        {
            get { return _apiAccessToken; }
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
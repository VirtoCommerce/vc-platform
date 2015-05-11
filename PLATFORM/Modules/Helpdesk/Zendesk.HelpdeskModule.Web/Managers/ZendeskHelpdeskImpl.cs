using Zendesk.HelpdeskModule.Web.Services;

namespace Zendesk.HelpdeskModule.Web.Managers
{
    public class ZendeskHelpdeskImpl: IHelpdesk
   {
        private readonly string _apiAccessToken;
        private readonly string _code;
        private readonly string _description;
        private readonly string _logoUrl;
        private readonly string _subdomain;

        public ZendeskHelpdeskImpl(string apiAccessToken, string code, string description, string logoUrl, string subdomain)
        {
            _apiAccessToken = apiAccessToken;
            _code = code;
            _description = description;
            _logoUrl = logoUrl;
            _subdomain = subdomain;
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

        public string Subdomain
        {
            get { return _subdomain; }
        }
    }
}
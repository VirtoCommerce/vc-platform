using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Zendesk.HelpdeskModule.Web.Services
{
    /// <summary>
    /// This is what gets returned from MailChimp to be used for the API.
    /// </summary>
    public class ZD_RestAPI
    {
        public string login_url { get; set; }
        public string api_endpoint { get; set; }
    }

    public class ZendeskOAuth: OAuth2
    {

        public ZendeskOAuth(Dictionary<string, string> p)
            : base(p)
        {

        }

        /// <summary>
        /// Gets the login URL based off cconfiguration settings.
        /// </summary>
        /// <returns>The login/Auth URL</returns>
        public string getLoginURL()
        {
            var p = new Dictionary<string, string>() { { "response_type", "code" }, { "client_id", _variables["client_id"] }, { "redirect_uri", _variables["redirect_uri"] }, {"scope", _variables["scope"] }, {"state", _variables["state"]} };
            return getURI(_variables["authorize_uri"], p);
        }

        /// <summary>
        /// Gets the information needed to complete the 
        /// </summary>
        /// <returns>The MailChimp REST_API configuration</returns>
        public ZD_RestAPI getMetaData()
        {
            var data = api("metadata", new Dictionary<string, string>());
            var serializer = new JavaScriptSerializer();
            var mc = serializer.Deserialize<ZD_RestAPI>(data);
            return mc;
        }
    }
}
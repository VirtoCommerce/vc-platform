using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
using System.Web.Routing;
using System.Web.SessionState;

namespace VirtoCommerce.Web.Client.Extensions.RouteHandlers
{
    public class CategoryRouteHandler : MvcRouteHandler
    {
        //Temp test data
        public static Dictionary<string, string> Mappings = new Dictionary<string, string> { 
        { "audio", "audio-mp3" }, 
        { "video", "tv-video" }, 
        { "computers", "computers-tablets" }, };

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var category = requestContext.RouteData.Values["category"].ToString();

            if (Mappings.ContainsKey(category))
            {
                category = Mappings[category];
            }

            //Category is accessded by code parameter
            requestContext.RouteData.Values["category"] = category;
            return base.GetHttpHandler(requestContext);
        }
    }
}

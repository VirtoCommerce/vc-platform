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
    public class ItemRouteHandler : MvcRouteHandler
    {
        //Temp test data
        public static Dictionary<string, string> Mappings = new Dictionary<string, string> { 
        { "Samsung-UN19D4003", "v-b004vrj3fq" }, 
        { "Samsung-UN22D5003", "v-b004vrj3e2" }, 
        { "Samsung-T23A350", "v-b004tptx0u" }, };
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var item = requestContext.RouteData.Values["item"].ToString();

            if (Mappings.ContainsKey(item))
            {
                item = Mappings[item];
            }

            //Category is accessded by code parameter
            requestContext.RouteData.Values["item"]=item;
            return base.GetHttpHandler(requestContext);
        }
    }
}

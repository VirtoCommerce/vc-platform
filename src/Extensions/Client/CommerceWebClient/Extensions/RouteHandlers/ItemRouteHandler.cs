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
    public class ItemRouteHandler : CategoryRouteHandler
    {
        //Temp test data
        public static Dictionary<string, string> ItemMappings = new Dictionary<string, string> { 
        { "Samsung-UN19D4003", "v-b004vrj3fq" }, 
        { "Samsung-UN22D5003", "v-b004vrj3e2" }, 
        { "Samsung-T23A350", "v-b004tptx0u" }, };
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var item = requestContext.RouteData.Values["item"].ToString();

            if (ItemMappings.Keys.Any(x => x.Equals(item, StringComparison.OrdinalIgnoreCase)))
            {
                item = ItemMappings.First(x => x.Key.Equals(item, StringComparison.OrdinalIgnoreCase)).Value;
            }

            requestContext.RouteData.Values["item"] = item;
            return base.GetHttpHandler(requestContext);
        }
    }
}

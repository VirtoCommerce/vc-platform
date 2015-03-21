using System;
using System.Web;
using System.Web.Routing;

namespace VirtoCommerce.Web.Client.Extensions.Routing.HttpHandlers
{
    public class DelegateHttpHandler : IHttpHandler
    {
        readonly Action<RequestContext> _action;
        readonly RouteData _routeData;

        public DelegateHttpHandler(Action<RequestContext> action, RouteData routeData, bool isReusable)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (routeData == null)
            {
                throw new ArgumentNullException("routeData");
            }

            IsReusable = isReusable;
            _action = action;
            _routeData = routeData;
        }

        public bool IsReusable
        {
            get;
            private set;
        }

        public void ProcessRequest(HttpContext context)
        {
            _action(new RequestContext(new HttpContextWrapper(context), _routeData));
        }
    }
}

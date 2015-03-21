using System.Web;
using System.Web.Routing;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.HttpHandlers
{
    public class RedirectHttpHandler : IHttpHandler, IRouteHandler
    {
        public RedirectHttpHandler(string targetUrl, bool permanent, bool isReusable)
        {
            TargetUrl = targetUrl;
            Permanent = permanent;
            IsReusable = isReusable;
        }

        public string TargetUrl
        {
            get;
            set;
        }

        public bool Permanent
        {
            get;
            private set;
        }

        public bool IsReusable
        {
            get;
            private set;
        }


        public void ProcessRequest(HttpContext context)
        {
            Redirect(context.Response, TargetUrl, Permanent);
        }

        private static void Redirect(HttpResponse response, string url, bool permanent)
        {
            if (permanent)
            {
                response.RedirectPermanent(url, true);
            }
            else
            {
                response.Redirect(url, false);
            }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }
    }
}

#region
using System.Web;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Routing.HttpHandlers
{
    public class RedirectHttpHandler : IHttpHandler, IRouteHandler
    {
        #region Constructors and Destructors
        public RedirectHttpHandler(string targetUrl, bool permanent, bool isReusable)
        {
            this.TargetUrl = targetUrl;
            this.Permanent = permanent;
            this.IsReusable = isReusable;
        }
        #endregion

        #region Public Properties
        public bool IsReusable { get; private set; }

        public bool Permanent { get; private set; }

        public string TargetUrl { get; set; }
        #endregion

        #region Public Methods and Operators
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        public void ProcessRequest(HttpContext context)
        {
            Redirect(context.Response, this.TargetUrl, this.Permanent);
        }
        #endregion

        #region Methods
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
        #endregion
    }
}
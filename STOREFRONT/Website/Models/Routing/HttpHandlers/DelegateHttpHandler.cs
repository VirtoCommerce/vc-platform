#region
using System;
using System.Web;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Routing.HttpHandlers
{
    public class DelegateHttpHandler : IHttpHandler
    {
        #region Fields
        private readonly Action<RequestContext> _action;

        private readonly RouteData _routeData;
        #endregion

        #region Constructors and Destructors
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

            this.IsReusable = isReusable;
            this._action = action;
            this._routeData = routeData;
        }
        #endregion

        #region Public Properties
        public bool IsReusable { get; private set; }
        #endregion

        #region Public Methods and Operators
        public void ProcessRequest(HttpContext context)
        {
            this._action(new RequestContext(new HttpContextWrapper(context), this._routeData));
        }
        #endregion
    }
}
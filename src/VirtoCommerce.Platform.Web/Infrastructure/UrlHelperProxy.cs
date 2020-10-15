using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace VirtoCommerce.Platform.Web.Infrastructure
{
    // Proxy class for easy implemetation IUrlHelper
    public class UrlHelperProxy : IUrlHelper
    {
        private readonly IActionContextAccessor accessor;
        private readonly IUrlHelperFactory factory;

        public UrlHelperProxy(IActionContextAccessor accessor, IUrlHelperFactory factory)
        {
            this.accessor = accessor;
            this.factory = factory;
        }

        public ActionContext ActionContext => UrlHelper.ActionContext;

        public string Action(UrlActionContext actionContext) => UrlHelper.Action(actionContext);

        public string Content(string contentPath) => UrlHelper.Content(contentPath);

        public bool IsLocalUrl(string url) => UrlHelper.IsLocalUrl(url);

        public string Link(string routeName, object values) => UrlHelper.Link(routeName, values);

        public string RouteUrl(UrlRouteContext routeContext) => UrlHelper.RouteUrl(routeContext);

        private IUrlHelper UrlHelper => factory.GetUrlHelper(accessor.ActionContext);
    }
}

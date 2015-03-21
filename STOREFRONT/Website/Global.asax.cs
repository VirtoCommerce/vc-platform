#region
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web
{
    public class MvcApplication : HttpApplication
    {
        #region Methods
        protected void Application_Start()
        {
            EnginesConfig.RegisterEngines(ViewEngines.Engines);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        #endregion
    }
}
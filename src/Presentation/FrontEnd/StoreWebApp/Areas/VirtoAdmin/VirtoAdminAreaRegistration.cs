using System.Web.Mvc;
using VirtoCommerce.Foundation.AppConfig;

namespace VirtoCommerce.Web.Areas.VirtoAdmin
{
    public class VirtoAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "VirtoAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            if (!AppConfigConfiguration.Instance.Setup.IsCompleted)
            {
                context.MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new {action = "Index", controller = "Install", id = UrlParameter.Optional}
                    );
            }
            else
            {
                context.MapRoute(
                    "AdminDefault",
                    "admin",
                    new { action = "Index", controller = "Install", id = UrlParameter.Optional }
                    );                
            }
        }
    }
}

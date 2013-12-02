using System.Web.Mvc;
using VirtoCommerce.Foundation.Data.Infrastructure;

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
            if (!ConnectionHelper.IsDatabaseInstalled)
            {
                context.MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new {action = "Index", controller = "Install", id = UrlParameter.Optional}
                    );

                
            }
        }
    }
}

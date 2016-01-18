using System.Web.Mvc;

namespace VirtoCommerce.Storefront
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "VirtoCommerce.Platform.Web.Controllers" }
            );
        }
    }
}

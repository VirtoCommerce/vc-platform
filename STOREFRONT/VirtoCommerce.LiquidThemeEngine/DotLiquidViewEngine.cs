using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using DotLiquid.ViewEngine.FileSystems;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Tags;

namespace VirtoCommerce.LiquidThemeEngine
{
    public class DotLiquidViewEngine : VirtualPathProviderViewEngine
    {
        public DotLiquidViewEngine()
        {
            // Register custom tags (Only need to do this once)
            Template.RegisterFilter(typeof(CommonFilters));
            Template.RegisterFilter(typeof(CommerceFilters));

            Template.RegisterTag<Layout>("layout");
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            Template.FileSystem = new ThemeLiquidFileSystem(HostingEnvironment.MapPath("~/App_Data/Theme"));

            return new DotLiquidView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            Template.FileSystem = new ThemeLiquidFileSystem("~/App_Data/Theme");

            return new DotLiquidView(controllerContext, viewPath, masterPath);
        }
    }
}

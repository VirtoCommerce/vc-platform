using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using DotLiquid.ViewEngine.FileSystems;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Operators;
using VirtoCommerce.LiquidThemeEngine.Tags;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine
{
    public class DotLiquidViewEngine : IViewEngine
    {
        private readonly string _viewBasePath;
        public DotLiquidViewEngine(string viewBasePath)
        {
            _viewBasePath = viewBasePath;
            Liquid.UseRubyDateFormat = true;
            // Register custom tags (Only need to do this once)
            Template.RegisterFilter(typeof(CommonFilters));
            Template.RegisterFilter(typeof(CommerceFilters));
            Template.RegisterFilter(typeof(TranslationFilter));

            Condition.Operators["contains"] = (left, right) => CommonOperators.ContainsMethod(left, right);

            Template.RegisterTag<Layout>("layout");
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            InitTemplate(controllerContext);
            return new ViewEngineResult(new DotLiquidView(controllerContext, partialViewName), this);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            InitTemplate(controllerContext);
            return new ViewEngineResult(new DotLiquidView(controllerContext, viewName, masterName), this);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
           
        }

        private void InitTemplate(ControllerContext controllerContext)
        {
            var workContext = controllerContext.Controller.ViewData.Model as WorkContext;
            Template.FileSystem = new ShopifyThemeLiquidFileSystem(Path.Combine(_viewBasePath, workContext.CurrentStore.ThemeName ?? "default"), workContext.CurrentLanguage);
        }
    }
}

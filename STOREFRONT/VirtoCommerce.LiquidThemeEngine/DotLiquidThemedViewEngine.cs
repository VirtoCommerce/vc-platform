using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.FileSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Operators;
using VirtoCommerce.LiquidThemeEngine.Tags;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine
{
  
    public class DotLiquidThemedViewEngine : IViewEngine
    {
        private ShopifyLiquidThemeStructure _themeAdaptor;

        public DotLiquidThemedViewEngine(ShopifyLiquidThemeStructure themeAdaptor)
        {
            _themeAdaptor = themeAdaptor;
        }

        #region IViewEngine members
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return new ViewEngineResult(new DotLiquidThemedView(_themeAdaptor, partialViewName, null), this);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return new ViewEngineResult(new DotLiquidThemedView(_themeAdaptor, viewName, masterName), this);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            //Nothing todo  
        }
        #endregion
    }
}

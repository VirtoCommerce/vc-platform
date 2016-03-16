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
        private ShopifyLiquidThemeEngine _themeEngine;

        public DotLiquidThemedViewEngine(ShopifyLiquidThemeEngine themeEngine)
        {
            _themeEngine = themeEngine;
        }

        #region IViewEngine members
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (_themeEngine.ResolveTemplatePath(partialViewName) != null)
            {
                return new ViewEngineResult(new DotLiquidThemedView(_themeEngine, partialViewName, null), this);
            }
            return new NullViewEngineResult();
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (String.IsNullOrEmpty(masterName))
            {
                masterName = _themeEngine.MasterViewName;
            }

            if (_themeEngine.ResolveTemplatePath(viewName) != null)
            {
                return new ViewEngineResult(new DotLiquidThemedView(_themeEngine, viewName, masterName), this);
            }
            return new NullViewEngineResult();
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            //Nothing todo  
        }
        #endregion
    }
    /// <summary>
    /// Class used to tell ASp.NET View locator what view not exist (because no other way to construct ViewEngineResult with null View)
    /// </summary>
    public class NullViewEngineResult : ViewEngineResult
    {
        public NullViewEngineResult()
            : base(new string[] { })
        {
            View = null;
        }
        public new IView View { get; private set; }
    }
}

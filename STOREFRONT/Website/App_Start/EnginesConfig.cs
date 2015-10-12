#region

using System.Collections.Generic;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using DotLiquid;
using DotLiquid.FileSystems;
using VirtoCommerce.Web.Models.Filters;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Models.Tags;
using VirtoCommerce.Web.Services;
using VirtoCommerce.Web.Views.Engines.Liquid;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.FileSystems;
using Tag = VirtoCommerce.Web.Models.Tagging.Tag;
using VirtoCommerce.Web.Filters;
using System.Collections;

#endregion

namespace VirtoCommerce.Web
{
    public class EnginesConfig
    {
        #region Public Methods and Operators
        public static void RegisterEngines(ViewEngineCollection engines)
        {
            Liquid.UseRubyDateFormat = true;
            Template.RegisterTag<Form>("form");
            Template.RegisterSafeType(typeof(Tag), o => { return o; });

            // Register custom contains condition
            Condition.Operators["contains"] = (left, right) => ContainsMethod(left, right);

            //Condition.Operators["contains"] = (left, right) => (left is ILiquidContains) ? ((ILiquidContains)left).Contains(right) : ((left is string) ? !(right == null || left == null) ? false : ((string)left).Contains((string)right) : false);

            var filters = new[] { typeof(ModelFilters), typeof(TranslationFilter) };
            var themesPath = ConfigurationManager.AppSettings["ThemeCacheFolder"];
            var viewLocator = new FileThemeViewLocator(themesPath);
            var viewParser = new LiquidTemplateParser(themesPath);
            engines.Add(new DotLiquidViewEngine(new DotLiquidFileSystemFactory(viewLocator), viewLocator, viewParser, filters));
        }
        #endregion

        public static bool ContainsMethod(object left, object right)
        {
            //return (left is ILiquidContains) ? ((ILiquidContains)left).Contains(right) : ((left is string) ? ((string)left).Contains(right as string != null ? right as string : string.Empty) : false);

            if (left is ILiquidContains)
            {
                return ((ILiquidContains)left).Contains(right);
            }

            if(left is string)
            {
                return right != null && ((string)left).Contains(right as string);
            }

            if(left is IList)
            {
                return ((IList)left).Contains(right);
            }

            return false;          
        }
    }

    public class DotLiquidFileSystemFactory : IFileSystemFactory
    {
        private readonly IViewLocator _locator;

        public DotLiquidFileSystemFactory(IViewLocator locator)
        {
            this._locator = locator;
        }

        #region Implementation of IFileSystemFactory

        /// <summary>
        /// Gets a <see cref="IFileSystem"/> instance for the provided <paramref>
        ///         <name>context</name>
        ///     </paramref>
        ///     .
        /// </summary>
        /// <param name="extensions">View extensions to search for</param>
        /// <returns>An <see cref="IFileSystem"/> instance.</returns>
        public IFileSystem GetFileSystem(IEnumerable<string> extensions)
        {
            return new ThemeFileSystem(_locator);
        }

        #endregion
    }
}
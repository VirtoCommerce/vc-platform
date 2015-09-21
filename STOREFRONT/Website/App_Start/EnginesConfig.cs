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
            Condition.Operators["contains"] = (left, right) => (left is ILiquidContains) ? ((ILiquidContains)left).Contains(right) : ((left is string) ? !(right == null || left == null) ? false : ((string)left).Contains((string)right) : false);

            var filters = new[] { typeof(ModelFilters), typeof(TranslationFilter) };
            var themesPath = ConfigurationManager.AppSettings["ThemeCacheFolder"];
            var viewLocator = new FileThemeViewLocator(themesPath);
            var viewParser = new LiquidTemplateParser(themesPath);
            engines.Add(new DotLiquidViewEngine(new DotLiquidFileSystemFactory(viewLocator), viewLocator, viewParser, filters));
        }
        #endregion
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
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DotLiquid;
using VirtoCommerce.Web.Views.Engines.Liquid.Tags;

namespace VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine
{
    public class DotLiquidViewEngine : IViewEngine
    {
        private readonly IViewLocator _viewLocator;
        private readonly IFileSystemFactory _fileSystem;
        public DotLiquidViewEngine(IFileSystemFactory fileSystem, IViewLocator viewLocator, IEnumerable<Type> filters)
        {
            DotLiquid.Liquid.UseRubyDateFormat = true;
            _viewLocator = viewLocator;
            this._fileSystem = fileSystem;
            // Register custom tags (Only need to do this once)
            Template.RegisterFilter(typeof(CommonFilters));
            Template.RegisterFilter(typeof(CommerceFilters));

            foreach (var filter in filters)
            {
                Template.RegisterFilter(filter);
            }

            Template.RegisterTag<Paginate>("paginate");
            Template.RegisterTag<CurrentPage>("current_page");
            Template.RegisterTag<Layout>("layout");
        }

        public IEnumerable<string> Extensions
        {
            get { yield return "liquid"; }
        }

        #region Implementation of IViewEngine

        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <returns>
        /// The partial view.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="partialViewName">The name of the partial view.</param><param name="useCache">true to specify that the view engine returns the cached view, if a cached view exists; otherwise, false.</param>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            Template.FileSystem = _fileSystem.GetFileSystem(this.Extensions);
            var view = _viewLocator.LocatePartialView(partialViewName);

            if (view.SearchedLocations != null)
            {
                return new ViewEngineResult(view.SearchedLocations);
            }

            return new ViewEngineResult(new DotLiquidView(controllerContext, _viewLocator, view), this);
        }

        /// <summary>
        /// Finds the specified view by using the specified controller context.
        /// </summary>
        /// <returns>
        /// The page view.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param><param name="viewName">The name of the view.</param><param name="masterName">The name of the master.</param><param name="useCache">true to specify that the view engine returns the cached view, if a cached view exists; otherwise, false.</param>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            Template.FileSystem = _fileSystem.GetFileSystem(this.Extensions);
            var view = _viewLocator.LocatePartialView(viewName);

            if (String.IsNullOrEmpty(masterName))
            {
                masterName = "theme";
            }

            var master = _viewLocator.LocateView(masterName);

            if (view.SearchedLocations != null)
            {
                return new ViewEngineResult(view.SearchedLocations);
            }

            return new ViewEngineResult(new DotLiquidView(controllerContext, _viewLocator, view, master), this);
        }

        /// <summary>
        /// Releases the specified view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param><param name="view">The view.</param>
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
        }

        #endregion
    }
}

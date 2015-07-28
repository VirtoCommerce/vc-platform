using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using VirtoCommerce.Web.Views.Engines.Liquid;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

namespace VirtoCommerce.Web.Services
{
    public class FileThemeViewLocator : IViewLocator
    {
        private readonly string _baseDirectoryPath;
        private VirtualPathProvider _vpp;

        public FileThemeViewLocator(string baseDirectoryPath)
        {
            this._baseDirectoryPath = baseDirectoryPath;

            this.ViewLocations = new[] { "layout", "templates", "snippets", "assets" };

            ViewLocationCache = new FileViewLocationCache();
        }

        protected VirtualPathProvider VirtualPathProvider
        {
            get
            {
                if (_vpp == null)
                {
                    _vpp = HostingEnvironment.VirtualPathProvider;
                }
                return _vpp;
            }
            set { _vpp = value; }
        }

        public FileViewLocationCache ViewLocationCache { get; set; }

        public IEnumerable<string> ViewLocations;
        public string FileNameFormat
        {
            get
            {
                return "{0}.liquid";
            }
        }

        private ViewLocationResult FindView(IEnumerable<string> locations, string viewName, string fileNameFormat = "{0}", bool throwException = false)
        {
            var checkedLocations = new List<string>();
            var viewFound = false;
            var context = new HttpContextWrapper(HttpContext.Current);

            var viewNameWithExtension = string.Format(fileNameFormat, viewName);

            ViewLocationResult foundView = null;

            var cacheKey = String.Format("{0}-{1}", this.ThemeDirectory, viewNameWithExtension);

            // check cache first
            foundView = ViewLocationCache.GetViewLocation(context, cacheKey) as ViewLocationResult;
            if (foundView != null) return foundView;

            foreach (var fullPath in locations.Select(viewLocation => Combine(this._baseDirectoryPath, this.ThemeDirectory, viewLocation, viewNameWithExtension)))
            {
                var file = VirtualPathProviderHelper.GetFile(fullPath);
                if (file != null)
                {
                    foundView = new FileViewLocationResult(file, file.VirtualPath);
                    viewFound = true;
                    break;
                }

                checkedLocations.Add(fullPath);
            }

            // now search in global location
            // App_Data/Themes/_Global
            if (!viewFound)
            {
                foreach (var fullPath in
                    locations.Select(
                        viewLocation => Combine(this._baseDirectoryPath, "_global", viewLocation, viewNameWithExtension)))
                {
                    var file = VirtualPathProviderHelper.GetFile(fullPath);
                    if (file != null)
                    {
                        foundView = new FileViewLocationResult(file, file.VirtualPath);
                        viewFound = true;
                        break;
                    }

                    checkedLocations.Add(Path.Combine(this._baseDirectoryPath, fullPath));
                }
            }

            if (foundView == null /*!viewFound && throwException*/)
            {
                foundView = new ViewLocationResult(checkedLocations.ToArray());
            }

            ViewLocationCache.InsertViewLocation(context, cacheKey, foundView);

            return foundView;
        }

        #region Implementation of IViewLocator

        public ViewLocationResult LocateView(string viewName)
        {
            var foundView = this.FindView(this.ViewLocations, viewName, this.FileNameFormat, true);
            return foundView;
        }

        public ViewLocationResult LocatePartialView(string viewName)
        {
            var foundView = this.FindView(this.ViewLocations, viewName, this.FileNameFormat, true);
            return foundView;
        }

        public ViewLocationResult LocateResource(string resourceName)
        {
            var foundView = this.LocateResource(resourceName, null);
            return foundView;
        }

        #endregion

        public ViewLocationResult LocateResource(string resourceName, IEnumerable<string> locations)
        {
            var foundView = this.FindView(locations ?? new[] { "config", "locales", "assets" }, resourceName);
            return foundView;
        }

        /// <summary>
        /// Move to IViewCache
        /// </summary>
        public void UpdateCache()
        {
        }

        private string Combine(params string[] paths)
        {
            return Path.Combine(paths).Replace("\\", "/").TrimStart('/');
        }

        protected virtual string BaseDirectory
        {
            get
            {
                return this._baseDirectoryPath;
            }
        }

        protected virtual string ThemeDirectory
        {
            get
            {
                //var fileSystemMainPath = Path.Combine(this._baseDirectoryPath, SiteContext.Current.Theme.Path);
                return SiteContext.Current.Theme.Path;
            }
        }
    }

    public class FileViewLocationCache
    {
        private static readonly TimeSpan _defaultTimeSpan = new TimeSpan(0, 15, 0);

        public FileViewLocationCache()
            : this(_defaultTimeSpan)
        {
        }

        public FileViewLocationCache(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
        }

        public TimeSpan TimeSpan { get; private set; }

        #region IViewLocationCache Members

        public object GetViewLocation(HttpContextBase httpContext, string key)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            return httpContext.Cache[key];
        }

        public void InsertViewLocation(HttpContextBase httpContext, string key, object view)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            CacheDependency dependency = null;
            if (view is FileViewLocationResult)
            {
                var virtualPath = (view as FileViewLocationResult).Location;
                var virtualPathDependencies = new List<string> { virtualPath };
                dependency = HostingEnvironment.VirtualPathProvider.GetCacheDependency(virtualPath, virtualPathDependencies, DateTime.UtcNow);
            }

            httpContext.Cache.Insert(key, view, dependency, Cache.NoAbsoluteExpiration, TimeSpan);
        }

        #endregion
    }
}
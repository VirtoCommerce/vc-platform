using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Views.Engines.Liquid;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Util;

namespace VirtoCommerce.Web.Services
{
    public class FileThemeViewLocator : IViewLocator
    {
        private readonly string _baseDirectoryPath;
        private readonly object _Lock = new object();

        public FileThemeViewLocator(string baseDirectoryPath)
        {
            this._baseDirectoryPath = baseDirectoryPath;

            this.ViewLocations = new[]
                                              {
                                                  "layout",
                                                  "templates",
                                                  "snippets",
                                                  "assets"
                                              };
        }

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

            var viewNameWithExtension = string.Format(fileNameFormat, viewName);

            FileViewLocationResult foundView = null;

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

            if (!viewFound && throwException)
            {
                return new ViewLocationResult(checkedLocations.ToArray());
            }

            // since file is stale, make sure to refresh cache contents

            if (foundView != null && foundView.IsStale())
            {
                lock (this._Lock)
                {
                    if (!foundView.Reload()) // file was deleted
                        return null;
                }
            }

            return foundView;
        }

        #region Implementation of IViewLocator

        public ViewLocationResult LocateView(string viewName)
        {
            var foundView = this.FindView(this.ViewLocations, viewName, this.FileNameFormat, true);
            if (foundView != null)
                return foundView;

            return null;
        }

        public ViewLocationResult LocatePartialView(string viewName)
        {
            var foundView = this.FindView(this.ViewLocations, viewName, this.FileNameFormat, true);
            if (foundView != null)
                return foundView;

            return null;
        }

        public ViewLocationResult LocateResource(string resourceName)
        {
            return this.LocateResource(resourceName, null);
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
            //this.LoadThemeFiles(true);
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
}
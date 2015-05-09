using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Views.Engines.Liquid;

namespace VirtoCommerce.Web.Models.Services
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
            var allFiles = this.LoadThemeFiles();
            var checkedLocations = new List<string>();
            var viewFound = false;

            ViewLocationResult foundView = null;
            foreach (var fullPath in
                locations.Select(
                    viewLocation => this.RemoveBaseDirectory(Path.Combine(this.ThemeDirectory, viewLocation, string.Format(fileNameFormat, viewName)))))
            {
                foundView = allFiles.SingleOrDefault(x => x.Name.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
                if (foundView != null)
                {
                    viewFound = true;
                    break;
                }

                checkedLocations.Add(Path.Combine(_baseDirectoryPath, fullPath));
            }

            // use more flex method
            if (!viewFound)
            {
                var tempViewName = string.Format(fileNameFormat, viewName);
                if (tempViewName.StartsWith("*"))
                {
                    tempViewName = viewName.RemovePrefix("*");
                    foreach (var location in locations)
                    {
                        var tempLocation = this.RemoveBaseDirectory(
                            Path.Combine(this.ThemeDirectory, location));
                        foundView = allFiles.SingleOrDefault(x => x.Name.EndsWith(tempViewName, StringComparison.OrdinalIgnoreCase) && x.Name.StartsWith(tempLocation));
                        if (foundView != null)
                        {
                            viewFound = true;
                            break;
                        }                       
                    }
                }
            }

            // now search in global location
            // App_Data/Themes/_Global
            if (!viewFound)
            {
                foreach (var fullPath in
                    locations.Select(
                        viewLocation => this.RemoveBaseDirectory(Path.Combine("_global", viewLocation, string.Format(fileNameFormat, viewName)))))
                {
                    foundView = allFiles.SingleOrDefault(x => x.Name.Equals(fullPath, StringComparison.OrdinalIgnoreCase));
                    if (foundView != null)
                    {
                        viewFound = true;
                        break;
                    }

                    checkedLocations.Add(Path.Combine(_baseDirectoryPath, fullPath));
                }
            }

            if (!viewFound && throwException)
            {
                return new ViewLocationResult(checkedLocations.ToArray());
            }

            // since file is stale, make sure to refresh cache contents
            if (foundView != null && foundView.IsStale())
            {
                if (foundView is FileViewLocationResult)
                {
                    lock (this._Lock)
                    {
                        if (!((FileViewLocationResult)foundView).Reload()) // file was deleted
                            return null;
                    }
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
            return LocateResource(resourceName, null);
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
            this.LoadThemeFiles(true);
        }

        private ViewLocationResult[] LoadThemeFiles(bool reload = false)
        {
            var contextKey = "vc-cms-files-" + ThemeDirectory;
            var value = !reload ? HttpRuntime.Cache.Get(contextKey) : null;

            if (value != null)
            {
                return value as ViewLocationResult[];
            }

            IEnumerable<ViewLocationResult> themeFiles = null;
            lock (this._Lock)
            {
                // check again, since it might have updated since last lock
                value = !reload ? HttpRuntime.Cache.Get(contextKey) : null;
                if (value != null)
                {
                    return value as ViewLocationResult[];
                }

                var directory = new DirectoryInfo(this.BaseDirectory);
                var allFiles = new List<FileInfo>(directory.GetFiles("*.*", SearchOption.AllDirectories));

                themeFiles = allFiles.Select(
                    x =>
                        new FileViewLocationResult(x, this.RemoveBaseDirectory(x.FullName)));

                HttpRuntime.Cache.Insert(contextKey, themeFiles.ToArray());
            }

            return themeFiles.ToArray();
        }

        private string RemoveBaseDirectory(string path)
        {
            return path.Replace(this.BaseDirectory, string.Empty).Replace("\\", "/").TrimStart('/');
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
                var fileSystemMainPath = Path.Combine(_baseDirectoryPath, SiteContext.Current.Theme.Path);
                return fileSystemMainPath;
            }
        }
    }
}
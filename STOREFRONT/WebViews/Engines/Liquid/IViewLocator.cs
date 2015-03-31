using System.Collections.Generic;

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public interface IViewLocator
    {
        ViewLocationResult LocateView(string viewName);

        ViewLocationResult LocatePartialView(string viewName);

        ViewLocationResult LocateResource(string resourceName);

        ViewLocationResult LocateResource(string resourceName, IEnumerable<string> locations);

        /// <summary>
        /// Move to IViewCache
        /// </summary>
        void UpdateCache();
    }
}

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public interface IViewLocator
    {
        ViewLocationResult LocateView(string viewName);

        ViewLocationResult LocatePartialView(string viewName);

        ViewLocationResult LocateResource(string resourceName);

        /// <summary>
        /// Move to IViewCache
        /// </summary>
        void UpdateCache();
    }
}

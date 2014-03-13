using System.Web.Mvc;

namespace VirtoCommerce.Web.Client.Caching.Interfaces
{
    public interface IDonutHoleFiller
    {
        /// <summary>
        /// Implentations should remove the donut hole wrappers.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="options">The output cache options.</param>
        /// <returns>A donut hole wrapper free string</returns>
        string RemoveDonutHoleWrappers(string content, ControllerContext filterContext, VirtoCommerce.Web.Client.Caching.OutputCacheOptions options);
        
        /// <summary>
        /// Replaces the donut holes content of with fresh content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="options">The output cache options.</param>
        /// <returns>A string containing the donut holes replaced by content.</returns>
        string ReplaceDonutHoleContent(string content, ControllerContext filterContext, VirtoCommerce.Web.Client.Caching.OutputCacheOptions options);
    }
}

#region
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Extensions
{
    public static class UrlHelperExtensions
    {
        #region Public Methods and Operators
        public static string ItemUrl(
            this UrlHelper helper,
            string itemKeyword,
            string categoryOutline,
            string parentId = null)
        {
            var routeValues = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(parentId))
            {
                routeValues.Add("item", parentId);
                routeValues.Add("variation", itemKeyword);
                routeValues.Add("category", categoryOutline);
            }
            else
            {
                routeValues.Add("item", itemKeyword);
                routeValues.Add("category", categoryOutline);
            }
            return helper.RouteUrl("Item", routeValues);
        }
        #endregion
    }
}
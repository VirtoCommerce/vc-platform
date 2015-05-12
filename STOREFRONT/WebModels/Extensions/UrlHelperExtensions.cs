#region

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Extensions
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
            return ItemUrl(helper, "Item", itemKeyword, categoryOutline, parentId);
        }

        /*
        public static string ItemUrlWithCode(
            this UrlHelper helper,
            string itemCode,
            string categoryOutline,
            string parentId = null)
        {
            return ItemUrl(helper, "ItemWithCode", itemCode, categoryOutline, parentId);
        }
         * */

        public static string ItemUrl(
            this UrlHelper helper,
            string routeName,
            string itemKeyword,
            string categoryOutline,
            string parentId)
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
            return helper.RouteUrl(routeName, routeValues);
        }

        public static string CategoryUrl(
            this UrlHelper helper,
            string categoryOutline,
            string parentId = null)
        {
            var routeValues = new RouteValueDictionary();

            if (!string.IsNullOrEmpty(parentId))
            {
                routeValues.Add("category", categoryOutline);
            }
            else
            {
                routeValues.Add("category", categoryOutline);
            }
            return helper.RouteUrl("category", routeValues);
        }
        #endregion
    }
}
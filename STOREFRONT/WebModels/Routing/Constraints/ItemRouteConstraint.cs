#region
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Caching;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Constraints
{
    /// <summary>
    ///     Route constraint checks if item exists in database
    /// </summary>
    public class ItemRouteConstraint : BaseRouteConstraint
    {
        #region Methods
        protected override bool IsMatch(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (!base.IsMatch(httpContext, route, parameterName, values, routeDirection))
            {
                return false;
            }

            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            var encoded = values[parameterName].ToString();
            var language = values.ContainsKey(Constants.Language)
                ? values[Constants.Language].ToString()
                : Thread.CurrentThread.CurrentUICulture.Name;
            var productSlug = encoded;

            var storeId = this.GetStoreId(values);
            if (storeId == null)
            {
                return false;
            }

            var itemRouteCacheKey = CacheKey.Create("ItemRouteConstraint", storeId, language, productSlug);
            var retVal = Task.Run(() => SiteContext.Current.CacheManager.GetAsync(itemRouteCacheKey, TimeSpan.FromHours(1), async () =>
            {
                var client = ClientContext.Clients.CreateBrowseClient();
                var item = await client.GetProductByKeywordAsync(storeId, language, productSlug, ItemResponseGroups.ItemMedium);
                if (item == null)
                {
                    item =await client.GetProductByCodeAsync(storeId, language, productSlug);
                    if (item == null)
                        return false;
                }
                return true;
            })).Result;


            return retVal;

        }
        #endregion
    }
}
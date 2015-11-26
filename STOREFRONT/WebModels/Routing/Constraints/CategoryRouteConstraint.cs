﻿#region
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Caching;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Constraints
{
    /// <summary>
    ///     Route constraint checks if category exists in database
    /// </summary>
    public class CategoryRouteConstraint : BaseRouteConstraint
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

            
            var categoryPath = values[parameterName].ToString();
            var childCategorySlug = categoryPath.Split(this.Separator.ToCharArray()).Last();
            var language = values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : Thread.CurrentThread.CurrentUICulture.Name;

            if (values.ContainsKey(Constants.Store))
            {
                
            }

            var storeId = this.GetStoreId(values);
            if (storeId == null)
            {
                return false;
            }

            var categoryRouteCacheKey = CacheKey.Create("CategoryRouteConstraint", storeId, language, childCategorySlug);
            var category = Task.Run(() => SiteContext.Current.CacheManager.GetAsync(categoryRouteCacheKey, TimeSpan.FromHours(1), async () =>
            {
                var client = ClientContext.Clients.CreateBrowseClient();
                var retVal = await client.GetCategoryByKeywordAsync(storeId, language, childCategorySlug);

                if (retVal == null)
                    retVal = await client.GetCategoryByCodeAsync(storeId, language, childCategorySlug);

                return retVal;

            })).Result;

        

            //var outline = category.AsWebModel().BuildOutline(language);
            //return category != null && this.ValidateCategoryPath(outline, categoryPath);

            return category != null;
        }
        #endregion
    }
}
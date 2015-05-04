#region
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;

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

            var client = ClientContext.Clients.CreateBrowseClient();
            var item = Task.Run(() => client.GetProductByKeywordAsync(storeId, language, productSlug, ItemResponseGroups.ItemMedium)).Result;

            if (item == null)
            {
                item = Task.Run(() => client.GetProductByCodeAsync(storeId, language, productSlug)).Result;
                if (item == null)
                    return false;
            }

            //Check if category is correct
            //if (values.ContainsKey(Constants.Category))
            //{
            //    encoded = values[Constants.Category].ToString();
            //    //var decoded = SettingsHelper.SeoDecode(encoded, SeoUrlKeywordTypes.Category, values.ContainsKey(Constants.Language) ? values[Constants.Language].ToString() : null);

            //    //Todo mark valid outline somehow
            //    return ValidateCategoryPath(item.Outline, decoded);
            //}

            return true;
        }
        #endregion
    }
}
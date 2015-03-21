using System.Collections.Generic;
using System.Text;
using System.Web.Routing;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;

namespace VirtoCommerce.ApiWebClient.Caching
{
    public class KeyBuilder : IKeyBuilder
    {
        private string _cacheKeyPrefix = "_d0nutc@che.";

        public string CacheKeyPrefix
        {
            get
            {
                return _cacheKeyPrefix;
            }
            set
            {
                _cacheKeyPrefix = value;
            }
        }

        public string BuildKey(string controllerName)
        {
            return BuildKey(controllerName, null, null);
        }

        public string BuildKey(string controllerName, string actionName)
        {
            return BuildKey(controllerName, actionName, null);
        }

        public string BuildKey(string controllerName, string actionName, RouteValueDictionary routeValues)
        {
            var builder = new StringBuilder(CacheKeyPrefix);

            if (controllerName != null)
            {
                builder.AppendFormat("{0}.", controllerName.ToLowerInvariant());
            }

            if (actionName != null)
            {
                builder.AppendFormat("{0}#", actionName.ToLowerInvariant());
            }

            if (routeValues != null)
            {
                foreach (var routeValue in routeValues)
                {
                    builder.Append(BuildKeyFragment(routeValue));
                }
            }

            return builder.ToString();
        }

        public string BuildKeyFragment(KeyValuePair<string, object> routeValue)
        {
            var value = routeValue.Value == null ? "<null>" : DecodeRouteValue(routeValue).ToLowerInvariant();

            return string.Format("{0}={1}#", routeValue.Key.ToLowerInvariant(), value);
        }

        private string DecodeRouteValue(KeyValuePair<string, object> routeValue)
        {
            //TODO need seo keywords api
            //switch (routeValue.Key)
            //{
            //    case Constants.Item:
            //        return SettingsHelper.SeoDecode(routeValue.Value.ToString(), SeoUrlKeywordTypes.Item);
            //    case Constants.Category:
            //        return SettingsHelper.SeoDecode(routeValue.Value.ToString(), SeoUrlKeywordTypes.Category);
            //    case Constants.Store:
            //        return SettingsHelper.SeoDecode(routeValue.Value.ToString(), SeoUrlKeywordTypes.Store);
            //    default:
                    return routeValue.Value.ToString();
            //}
        }
    }
}

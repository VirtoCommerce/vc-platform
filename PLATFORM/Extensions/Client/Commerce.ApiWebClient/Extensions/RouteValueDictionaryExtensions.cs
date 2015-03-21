using System.Web.Routing;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        const string RouteNameKey = "__RouteName";

        public static RouteValueDictionary SetRouteName(this RouteValueDictionary routeValues, string routeName)
        {
            if (routeValues == null)
            {
                return null;
            }
            routeValues[RouteNameKey] = routeName;
            return routeValues;
        }

        public static RouteValueDictionary WithoutRouteName(this RouteValueDictionary routeValues)
        {
            routeValues = new RouteValueDictionary(routeValues);
            routeValues.Remove(RouteNameKey);
            return routeValues;
        }

        public static string GetRouteName(this RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                return null;
            }
            object routeName;
            routeValues.TryGetValue(RouteNameKey, out routeName);
            return routeName as string;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary routeValues, RouteValueDictionary targetRouteValues)
        {
            if (routeValues == null)
            {
                return new RouteValueDictionary(targetRouteValues);
            }

            if (targetRouteValues == null)
            {
                return new RouteValueDictionary(routeValues);
            }

            var mergedRouteValues = new RouteValueDictionary(routeValues);
            // Target values take precedence.
            foreach (var key in targetRouteValues.Keys)
            {
                mergedRouteValues[key] = targetRouteValues[key];
            }
            return mergedRouteValues;
        }

        public static RouteValueDictionary ConvertToRouteValueDictionary(object routeValues)
        {
            if (routeValues == null)
            {
                return null;
            }
            var routeValueDictionary = routeValues as RouteValueDictionary;
            return routeValueDictionary ?? new RouteValueDictionary(routeValues);
        }

        
    }
}

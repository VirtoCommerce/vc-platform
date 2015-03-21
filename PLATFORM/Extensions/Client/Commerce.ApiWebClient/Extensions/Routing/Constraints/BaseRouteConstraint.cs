using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace VirtoCommerce.ApiWebClient.Extensions.Routing.Constraints
{
    public class BaseRouteConstraint : IRouteConstraint
    {
        protected string Separator
        {
            get
            {
                return "/";
            }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return IsMatch(httpContext, route, parameterName, values, routeDirection);
        }

        protected virtual bool IsMatch(HttpContextBase httpContext, Route route, string parameterName,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values.ContainsKey(parameterName) && values[parameterName] as string != null;
        }


        /// <summary>
        /// Validates the category path.
        /// Should allow categories to match in same order. 
        /// Can only allow some missings segments
        /// </summary>
        /// <param name="outline">The outline containing ids and semantic url mappings.</param>
        /// <param name="requestedPath">The requested path. Containing category codes code1/code2/.../codeN</param>
        /// <returns></returns>
        protected virtual bool ValidateCategoryPath(Dictionary<string, string> outline, string requestedPath)
        {
            var prevCatIndex = -1;
            var outlineIds = outline.Keys.ToList();
            foreach (var segment in requestedPath.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var category =
                    outline.FirstOrDefault(
                        x =>
                            x.Key.Equals(segment, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Value.Equals(segment, StringComparison.InvariantCultureIgnoreCase));

                //Category must exist
                if (category.Equals(default(KeyValuePair<string, string>)))
                {
                    return false;
                }

                var currentCatIndex = outlineIds.IndexOf(category.Key);

                //Segments order must match outline order
                if (prevCatIndex > 0 && prevCatIndex > currentCatIndex)
                {
                    return false;
                }

                prevCatIndex = currentCatIndex;
            }

            return true;
        }
    }
}

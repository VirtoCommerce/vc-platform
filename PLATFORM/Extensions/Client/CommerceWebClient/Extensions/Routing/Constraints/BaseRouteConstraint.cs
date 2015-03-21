using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.Catalogs.Services;

namespace VirtoCommerce.Web.Client.Extensions.Routing.Constraints
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
        /// <param name="outline">The outline.</param>
        /// <param name="requestedPath">The requested path. Containing category codes code1/code2/.../codeN</param>
        /// <returns></returns>
        protected virtual bool ValidateCategoryPath(string outline, string requestedPath)
        {
            var prevCatIndex = -1;
            foreach (var segment in requestedPath.Split(Separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var categories = outline.Split(Separator.ToCharArray()).ToList();
                var category = categories.FirstOrDefault(c => c.Equals(segment, StringComparison.InvariantCultureIgnoreCase));

                //Category must exist
                if (category == null)
                {
                    return false;
                }
                var currentCatIndex = categories.IndexOf(category);

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

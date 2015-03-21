#region
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

#endregion

namespace VirtoCommerce.Web.Models.Routing.Constraints
{
    /// <summary>
    ///     Route constraint checks if language is correct
    /// </summary>
    public class LanguageRouteConstraint : BaseRouteConstraint
    {
        #region Methods
        protected override bool IsMatch(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
            {
                return true;
            }

            if (!base.IsMatch(httpContext, route, parameterName, values, routeDirection))
            {
                return false;
            }

            //First check that language matches regex pattern
            object parameterValue;
            values.TryGetValue(parameterName, out parameterValue);
            var parameterValueString = Convert.ToString(parameterValue, CultureInfo.InvariantCulture);
            var constraintsRegEx = string.Format("^({0})$", Constants.LanguageRegex);
            if (
                !Regex.IsMatch(
                    parameterValueString,
                    constraintsRegEx,
                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled))
            {
                return false;
            }

            try
            {
                var culture = CultureInfo.CreateSpecificCulture(parameterValueString);
                //If culture is created then it is correct
            }
            catch
            {
                //Language is not valid
                return false;
            }

            return true;
        }
        #endregion
    }
}
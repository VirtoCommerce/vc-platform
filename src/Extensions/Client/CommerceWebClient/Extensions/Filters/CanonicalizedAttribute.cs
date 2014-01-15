using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Filters
{
    public class CanonicalizedAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            if (context.Request.Url != null)
            {
                var path = HttpUtility.UrlDecode(context.Request.Url.AbsolutePath);

                if (!string.IsNullOrEmpty(path))
                {
                    var query = context.Request.Url.Query;
                    var queryString = context.Request.QueryString;
                    var needRedirect = false;

                    // don't 'rewrite' POST requests
                    if (context.Request.RequestType == "GET" && !filterContext.IsChildAction)
                    {
                        //Process query string
                        if (!string.IsNullOrEmpty(query))
                        {
                            //Rebuild querystring from scratch
                            var newQuery = string.Empty;

                            //First goes search filter ordered based on document
                            var helper = new SearchHelper(StoreHelper.StoreClient.GetCurrentStore());
                            var urlHelper = new UrlHelper(context.Request.RequestContext);

                            var parameters = helper.Filters.Select(filter => queryString.AllKeys
                                .FirstOrDefault(k => k.Equals(urlHelper.GetFacetKey(filter.Key), StringComparison.InvariantCultureIgnoreCase)))
                                .Where(key => !string.IsNullOrEmpty(key))
                                .ToDictionary<string, string, object>(key => key, key => queryString[key]);

                            if (parameters.Any())
                            {
                                newQuery = urlHelper.SetQueryParameters(newQuery, parameters);
                            }

                            //Order remaining parameters
                            var otherParams = queryString.AllKeys.Where(key => !parameters.ContainsKey(key)).OrderBy(k => k)
                                .ToDictionary<string, string, object>(key => key, key => queryString[key]);

                            if (otherParams.Any())
                            {
                                newQuery = urlHelper.SetQueryParameters(newQuery, otherParams);
                            }

                            if (!string.IsNullOrEmpty(newQuery) && !newQuery.StartsWith("?"))
                            {
                                newQuery = string.Concat("?", newQuery);
                            }

                            if (!string.Equals(query, newQuery))
                            {
                                query = newQuery;
                                needRedirect = true;
                            }
                        }

                      
                        //make language code allways be five symbols
                        if (filterContext.RouteData.Values.ContainsKey(Routing.Constants.Language) &&
                            filterContext.RouteData.Values[Routing.Constants.Language] as string != null)
                        {
                            var lang = filterContext.RouteData.Values[Routing.Constants.Language].ToString();
                            if (lang.Length < 5)
                            {
                                try
                                {
                                    var cult = CultureInfo.CreateSpecificCulture(lang);
                                    if (!path.ToLowerInvariant().Contains(cult.Name.ToLowerInvariant()))
                                    {
                                        path = path.Replace(lang, cult.Name);
                                        needRedirect = true;
                                    }
                                }
                                catch
                                {
                                    //Something wrong with language??
                                }
                            }
                        }

                        //make path segments allways encoded
                        var encodedPath = path;
                        encodedPath = ProcessSegment(filterContext.RouteData.Values, encodedPath, Routing.Constants.Store);
                        encodedPath = ProcessSegment(filterContext.RouteData.Values, encodedPath, Routing.Constants.Category);
                        encodedPath = ProcessSegment(filterContext.RouteData.Values, encodedPath, Routing.Constants.Item);

                        // check for any upper-case letters:
                        if (path != encodedPath.ToLowerInvariant())
                        {
                            path = encodedPath.ToLowerInvariant();
                            needRedirect = true;
                        }

                        // make sure request ends with a "/"
                        if (path.EndsWith("/"))
                        {
                            needRedirect = true;
                        }
                    }

                    if (needRedirect)
                    {
                        Redirect(context, path, query);
                        return;
                    }
                }
            }

            base.OnActionExecuting(filterContext);

        }

        private string ProcessSegment(RouteValueDictionary routeValues, string path, string key)
        {
            SeoUrlKeywordTypes type;

            if (routeValues.ContainsKey(key) &&
                          routeValues[key] as string != null && Enum.TryParse(key,true, out type))
            {
                var value = routeValues[key].ToString();
                var valueDecoded = SettingsHelper.SeoEncode(value, type).ToLowerInvariant();

                //If encoded and used value does not match (keyword exist) and requested path contains not encoded value replace with encoded
                if (!value.Equals(valueDecoded, StringComparison.InvariantCultureIgnoreCase) && path.ToLowerInvariant().Contains(value.ToLowerInvariant()))
                {
                    path = path.Replace(value, valueDecoded);
                }
            }

            return path;
        }



        // correct as many 'rules' as possible per redirect to avoid
        // issuing too many redirects per request.
        private void Redirect(HttpContextBase context, string path, string query)
        {
            var newLocation = path;

            if (newLocation.EndsWith("/"))
                newLocation = newLocation.Substring(0, newLocation.Length - 1);

            newLocation = newLocation.ToLower(CultureInfo.InvariantCulture);

            context.Response.RedirectPermanent(newLocation + query, true);
        }

    }
}
using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

namespace VirtoCommerce.Web.Models.Routing
{
    public class CanonicalizedAttribute : ActionFilterAttribute
    {
        #region Fields

        private readonly Type[] _skipControllers;
        private readonly bool _usePermanentRedirect;

        #endregion

        #region Constructors and Destructors

        public CanonicalizedAttribute(params Type[] skipControllers)
            : this(true, skipControllers)
        {
        }

        public CanonicalizedAttribute(bool usePermanentRedirect, params Type[] skipControllers)
        {
            this._usePermanentRedirect = usePermanentRedirect;
            this._skipControllers = skipControllers;
        }

        #endregion

        #region Public Methods and Operators

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this._skipControllers.Any(x => filterContext.ActionDescriptor.ControllerDescriptor.ControllerType == x))
            {
                return;
            }
            var context = filterContext.HttpContext;

            // don't 'rewrite' POST requests, child action and ajax requests
            if (context.Request.RequestType != "GET" || filterContext.IsChildAction || context.Request.IsAjaxRequest())
            {
                return;
            }

            if (context.Request.Url != null)
            {
                var baseUri = string.Format(
                    "{0}://{1}{2}",
                    context.Request.Url.Scheme,
                    context.Request.Url.Host,
                    context.Request.Url.Port == 80 ? "" : ":" + context.Request.Url.Port);

                //var path = HttpUtility.UrlDecode(string.Concat(baseUri, context.Request.Url.AbsolutePath));
                var path = string.Concat(baseUri, context.Request.Url.AbsolutePath);

                if (!string.IsNullOrEmpty(path))
                {
                    var query = HttpUtility.UrlDecode(context.Request.Url.Query);
                    var queryString = context.Request.QueryString;
                    var needRedirect = false;

                    //make language code allways be five symbols
                    if (filterContext.RouteData.Values.ContainsKey(Constants.Language) &&
                        filterContext.RouteData.Values[Constants.Language] as string != null)
                    {
                        var lang = filterContext.RouteData.Values[Constants.Language].ToString();
                        if (lang.Length < 5)
                        {
                            try
                            {
                                var cult = CultureInfo.CreateSpecificCulture(lang);
                                if (!lang.Equals(cult.Name, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    filterContext.RouteData.Values[Constants.Language] =
                                        cult.Name.ToLowerInvariant();
                                    needRedirect = true;
                                }
                            }
                            catch
                            {
                                //Something wrong with language??
                            }
                        }
                    }

                    //Make sure we allways use same virtual path as Route provides

                    if (filterContext.RouteData.Route != null)
                    {
                        var routePath = filterContext.RouteData.Route.GetVirtualPath(
                            filterContext.RequestContext, filterContext.RouteData.Values);

                        if (routePath != null && !string.IsNullOrEmpty(routePath.VirtualPath))
                        {
                            var absoluteRoutePath = HttpUtility.UrlDecode(string.Concat(baseUri, context.Request.ApplicationPath, context.Request.ApplicationPath != "/" ? "/" : "", routePath.VirtualPath));

                            if (!string.IsNullOrEmpty(absoluteRoutePath) && !absoluteRoutePath.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                            {
                                path = absoluteRoutePath;
                                needRedirect = true;
                            }
                        }
                    }

                    //Process query string
                    if (!string.IsNullOrEmpty(query))
                    {
                        //Rebuild querystring from scratch
                        var newQuery = string.Empty;

                        //First goes search filter ordered based on document
                        //var helper = StoreHelper.SearchFilter;
                        var urlHelper = new UrlHelper(context.Request.RequestContext);

                        //TODO handle filters parameters
                        //var parameters = helper.Filters.Where(f => !(f is PriceRangeFilter) || ((PriceRangeFilter)f).Currency.Equals(StoreHelper.CustomerSession.Currency, StringComparison.OrdinalIgnoreCase))
                        //    .Select(filter => queryString.AllKeys
                        //    .FirstOrDefault(k => k.Equals(urlHelper.GetFacetKey(filter.Key), StringComparison.InvariantCultureIgnoreCase)))
                        //    .Where(key => !string.IsNullOrEmpty(key))
                        //    .ToDictionary<string, string, object>(key => key, key => queryString[key]);

                        //if (parameters.Any())
                        //{
                        //    newQuery = urlHelper.SetQueryParameters(newQuery, parameters);
                        //}

                        //Order remaining parameters
                        var otherParams = queryString.AllKeys.Where(k => k != null) /*.Where(key => !parameters.ContainsKey(key)).OrderBy(k => k)*/
                            .ToDictionary<string, string, object>(key => key, key => queryString[key]);

                        if (otherParams.Any())
                        {
                            newQuery = urlHelper.SetQueryParameters(newQuery, otherParams);
                        }

                        if (!string.IsNullOrEmpty(newQuery) && !newQuery.StartsWith("?"))
                        {
                            newQuery = string.Concat("?", newQuery);
                        }

                        newQuery = HttpUtility.UrlDecode(newQuery);

                        if (!string.Equals(query, newQuery, StringComparison.InvariantCultureIgnoreCase))
                        {
                            query = newQuery;
                            needRedirect = true;
                        }
                    }

                    // check for any upper-case letters:
                    if (path != path.ToLowerInvariant())
                    {
                        path = path.ToLowerInvariant();
                        needRedirect = true;
                    }

                    //If request is for root dont chech this rule
                    if (!context.Request.Url.AbsolutePath.Equals(context.Request.ApplicationPath))
                    {
                        // make sure request ends with a "/"
                        if (path.EndsWith("/"))
                        {
                            needRedirect = true;
                        }
                    }

                    if (needRedirect)
                    {
                        this.Redirect(context, path, query);
                        return;
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion

        // correct as many 'rules' as possible per redirect to avoid
        // issuing too many redirects per request.

        #region Methods

        private void Redirect(HttpContextBase context, string path, string query)
        {
            var newLocation = path;

            if (newLocation.EndsWith("/"))
            {
                newLocation = newLocation.Substring(0, newLocation.Length - 1);
            }

            newLocation = newLocation.ToLower(CultureInfo.InvariantCulture);

            if (this._usePermanentRedirect)
            {
                context.Response.RedirectPermanent(newLocation + query, true);
            }
            else
            {
                context.Response.Redirect(newLocation + query, true);
            }
        }

        #endregion
    }
}

#region

using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Search.Providers.Elastic;
using VirtoCommerce.Web.Client.Extensions;

#endregion

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class FilterConfig.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleAndLogErrorAttribute());
        }
    }

    /// <summary>
    /// Class HandleAndLogErrorAttribute.
    /// </summary>
    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The action-filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.IsChildAction ||
                filterContext.ExceptionHandled ||
                !filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var errorPage = "";
            var exception = filterContext.Exception as HttpException;
            if (exception != null)
            {
                statusCode = exception.GetHttpCode();
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                //to prevent login prompt in IIS
                // which will appear when returning 401.
                statusCode = (int)HttpStatusCode.Forbidden;
            }

            if (filterContext.Exception.Is<SearchException>())
            {
                errorPage = "~/Views/Error/SearchError.cshtml";
            }

            var result = CreateActionResult(filterContext, statusCode, errorPage);

            filterContext.Result = result;

            //// Prepare the response code.
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        /// <summary>
        /// Creates the action result.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="errorPage">The error page.</param>
        /// <returns>ActionResult.</returns>
        protected virtual ActionResult CreateActionResult(ExceptionContext filterContext, int statusCode,
                                                          string errorPage = "")
        {
            var ctx = new ControllerContext(filterContext.RequestContext, filterContext.Controller);

            var specificPage = errorPage;

            if (string.IsNullOrEmpty(specificPage))
            {
                switch (statusCode)
                {
                    case 403:
                        specificPage = "~/Views/Error/StoreClosed.cshtml";
                        break;
                    case 404:
                        specificPage = "~/Views/Error/Oops.cshtml";
                        break;
                }
            }


            var viewName = SelectFirstView(ctx, specificPage, string.Format("~/Views/Error/{0}.cshtml", statusCode),
                                           "~/Views/Error/Oops.cshtml", "Error");

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var result = new ViewResult
                {
                    ViewName = viewName,
                    MasterName = "~/Views/Shared/_ErrorLayout.cshtml",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
                };
            result.ViewBag.StatusCode = statusCode;
            return result;
        }

        /// <summary>
        /// Selects the first view.
        /// </summary>
        /// <param name="ctx">The ControllerContext.</param>
        /// <param name="viewNames">The view names.</param>
        /// <returns>System.String.</returns>
        protected string SelectFirstView(ControllerContext ctx, params string[] viewNames)
        {
            return viewNames.First(view => !string.IsNullOrWhiteSpace(view) && ViewExists(ctx, view));
        }

        /// <summary>
        /// Views the exists.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if view exist, <c>false</c> otherwise.</returns>
        protected bool ViewExists(ControllerContext ctx, string name)
        {
            var result = ViewEngines.Engines.FindView(ctx, name, null);
            return result.View != null;
        }
    }
}
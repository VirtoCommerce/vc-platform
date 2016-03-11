using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Common
{
    /// <summary>
    /// Use attribute to handle exception in called by AJAX controllers
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HandleJsonErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentException("filterContext");

            if (filterContext.IsChildAction)
            {
                return;
            }

            // execute so that the user can see useful debugging information. 
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            var exception = filterContext.Exception;

            var httpStatusCode = HttpStatusCode.InternalServerError;
            if (exception is HttpException)
            {
                httpStatusCode = (HttpStatusCode)((HttpException)exception).GetHttpCode();
            }

            // Grab all the error messages.
            var errorData = new
            {
                message = filterContext.Exception.Message,
                stackTrace = filterContext.Exception.ToString()
            };

            filterContext.Result = new JsonResult()
            {
                Data = errorData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)httpStatusCode;
            // Certain versions of IIS will sometimes use their own error page when 
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
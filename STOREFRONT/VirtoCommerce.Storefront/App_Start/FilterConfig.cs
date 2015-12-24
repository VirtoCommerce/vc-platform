using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web.Mvc;
using VirtoCommerce.Storefront.Model;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Web.Routing;

namespace VirtoCommerce.Storefront.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new JsonNetActionFilter());
            filters.Add(new StorefrontValidationActionFilter());
        }
    }
    public class StorefrontValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var workContext = UnityConfig.GetConfiguredContainer().Resolve<WorkContext>();
            if ((workContext.AllStores == null || !workContext.AllStores.Any()) && filterContext.ActionDescriptor.ActionName != "NoStoreFound")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Common", action = "NoStore" }));
            }
            base.OnActionExecuting(filterContext);
        }
      
    }

    public class JsonNetActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var jsonResult = filterContext.Result as JsonResult;

            if (jsonResult != null)
            {
                filterContext.Result = new JsonNetResult
                {
                    ContentEncoding = jsonResult.ContentEncoding,
                    ContentType = jsonResult.ContentType,
                    Data = jsonResult.Data,
                    JsonRequestBehavior = jsonResult.JsonRequestBehavior
                };
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };

            Settings.Converters.Add(new StringEnumConverter
            {
                AllowIntegerValues = false,
                CamelCaseText = false
            });
        }

        public JsonSerializerSettings Settings { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data == null)
            {
                return;
            }

            var scriptSerializer = JsonSerializer.Create(Settings);

            scriptSerializer.Serialize(response.Output, Data);
        }
    }
}
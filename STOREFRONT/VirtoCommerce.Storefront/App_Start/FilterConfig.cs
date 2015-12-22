using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web.Mvc;

namespace VirtoCommerce.Storefront.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new JsonNetActionFilter());
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
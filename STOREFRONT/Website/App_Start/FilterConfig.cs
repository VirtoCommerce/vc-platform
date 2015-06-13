#region
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using VirtoCommerce.Web.Views.Engines.Liquid;

#endregion

namespace VirtoCommerce.Web
{
    public class FilterConfig
    {
        #region Public Methods and Operators
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute { View = "~/Errors" });
            filters.Add(new JsonNetActionFilter());
        }
        #endregion
    }

    public class JsonNetResult : JsonResult
    {
        #region Constructors and Destructors
        public JsonNetResult()
        {
            this.Settings = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                                ContractResolver = new RubyContractResolver()
                            };
        }
        #endregion

        #region Public Properties
        public JsonSerializerSettings Settings { get; private set; }
        #endregion

        #region Public Methods and Operators
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data == null)
            {
                return;
            }

            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            var scriptSerializer = JsonSerializer.Create(this.Settings);
            // Serialize the data to the Output stream of the response
            scriptSerializer.Serialize(response.Output, this.Data);
        }
        #endregion
    }

    public class JsonNetActionFilter : ActionFilterAttribute
    {
        #region Public Methods and Operators
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result.GetType() == typeof(JsonResult))
            {
                // Get the standard result object with unserialized data
                var result = filterContext.Result as JsonResult;

                // Replace it with our new result object and transfer settings
                filterContext.Result = new JsonNetResult
                                       {
                                           ContentEncoding = result.ContentEncoding,
                                           ContentType = result.ContentType,
                                           Data = result.Data,
                                           JsonRequestBehavior = result.JsonRequestBehavior
                                       };

                // Later on when ExecuteResult will be called it will be the
                // function in JsonNetResult instead of in JsonResult
            }
            base.OnActionExecuted(filterContext);
        }
        #endregion
    }
}
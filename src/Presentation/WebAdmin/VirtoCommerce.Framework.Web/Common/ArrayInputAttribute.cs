using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.ObjectBuilder2;

namespace VirtoCommerce.Framework.Web.Common
{
    public class ArrayInputAttribute : ActionFilterAttribute
    {
        private readonly string[] _parameterNames;
        /// <summary>
        /// 
        /// </summary>
        public string Separator { get; set; }
        /// <summary>
        /// cons
        /// </summary>
        /// <param name="parameterName"></param>
        public ArrayInputAttribute(params string[] parameterName)
        {
            this._parameterNames = parameterName;
            Separator = ",";
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessArrayInput(HttpActionContext actionContext, string parameterName)
        {
            if (actionContext.ActionArguments.ContainsKey(parameterName))
            {
                var parameterDescriptor = actionContext.ActionDescriptor.GetParameters().FirstOrDefault(p => p.ParameterName == parameterName);
                if (parameterDescriptor != null && parameterDescriptor.ParameterType.IsArray)
                {
                    var type = parameterDescriptor.ParameterType.GetElementType();
                    var parameters = String.Empty;
                    if (actionContext.ControllerContext.RouteData.Values.ContainsKey(parameterName))
                    {
                        parameters = (string)actionContext.ControllerContext.RouteData.Values[parameterName];
                    }
                    else
                    {
                        var queryString = actionContext.ControllerContext.Request.RequestUri.ParseQueryString();
                        if (queryString[parameterName] != null)
                        {
                            parameters = queryString[parameterName];
                        }
                    }

                    var values = parameters.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(TypeDescriptor.GetConverter(type).ConvertFromString).ToArray();
                    var typedValues = Array.CreateInstance(type, values.Length);
                    values.CopyTo(typedValues, 0);
                    actionContext.ActionArguments[parameterName] = typedValues;
                }
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            this._parameterNames.ForEach(parameterName => ProcessArrayInput(actionContext, parameterName));
        }
    }
}

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
        #region Static Fields

        private static readonly string[] _emptyArray = new string[0];

        #endregion

        #region Fields

        private string _parameterName;
        private string[] _parameterNames = _emptyArray;

        #endregion

        #region Constructors and Destructors

        public ArrayInputAttribute()
        {
            ValueSeparator = ",";
        }

        #endregion

        #region Public Properties

        public string ParameterName
        {
            get { return _parameterName ?? string.Empty; }
            set
            {
                _parameterName = value;
                _parameterNames = this.SplitString(value, ',');
            }
        }

        public string ValueSeparator { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _parameterNames.ForEach(parameterName => ProcessArrayInput(actionContext, parameterName));
        }

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

                    var values = parameters.Split(new[] { ValueSeparator }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(TypeDescriptor.GetConverter(type).ConvertFromString).ToArray();
                    var typedValues = Array.CreateInstance(type, values.Length);
                    values.CopyTo(typedValues, 0);
                    actionContext.ActionArguments[parameterName] = typedValues;
                }
            }
        }

        #endregion
    }
}

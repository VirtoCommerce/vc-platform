#region
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Extensions
{
    public static class TemplateExtensions
    {
        #region Public Methods and Operators
        public static string RenderWithTracing(this Template template, RenderParameters parameters)
        {
            var content = template.Render(parameters);
            if (template.Errors.Any())
            {
                template.Errors.ForEach(e => Trace.TraceError(FlattenException(e)));
            }

            return content;
        }

        public static void RenderWithTracing(this Template template, TextWriter result, RenderParameters parameters)
        {
            template.Render(result, parameters);
            if (template.Errors.Any())
            {
                template.Errors.ForEach(e => Trace.TraceError(FlattenException(e)));
            }
        }
        #endregion

        #region Methods
        private static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
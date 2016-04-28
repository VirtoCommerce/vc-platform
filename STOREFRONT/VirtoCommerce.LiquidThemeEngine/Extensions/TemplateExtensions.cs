using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Extensions
{
    public static class TemplateExtensions
    {
        #region Public Methods and Operators
        public static string RenderWithTracing(this Template template, RenderParameters parameters)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            string content = null;
            try
            {
                content = template.Render(parameters);
            }
            catch(Exception ex)
            {
                Trace.TraceError(FlattenException(ex));
            }
            finally
            {
                if (template.Errors.Any())
                {
                    template.Errors.ForEach(e => Trace.TraceError(FlattenException(e)));
                }
            }
            return content;
        }

        public static void RenderWithTracing(this Template template, TextWriter result, RenderParameters parameters)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            try
            {
                template.Render(result, parameters);
            }
            catch (Exception ex)
            {
                Trace.TraceError(FlattenException(ex));
            }
            finally
            {
                if (template.Errors.Any())
                {
                    template.Errors.ForEach(e => Trace.TraceError(FlattenException(e)));
                }
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

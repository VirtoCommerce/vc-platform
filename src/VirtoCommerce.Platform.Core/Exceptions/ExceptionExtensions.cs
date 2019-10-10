using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.Platform.Core.Exceptions
{
    public static class ExceptionExtensions
    {
        public static void ThrowFaultException(this Exception ex)
        {
            throw new Exception(ex.ExpandExceptionMessage());
        }

        public static string ExpandExceptionMessage(this Exception ex)
        {
            var builder = new StringBuilder();

            var separator = Environment.NewLine;
            var exception = ex;

            while (exception != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }
                builder.Append(exception.Message);
                exception = exception.InnerException;
            }

            return builder.ToString();
        }
    }
}

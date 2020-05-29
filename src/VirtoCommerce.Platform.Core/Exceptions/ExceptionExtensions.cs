using System;
using System.Text;

namespace VirtoCommerce.Platform.Core.Exceptions
{
    public static class ExceptionExtensions
    {
        public static void ThrowFaultException(this Exception ex)
        {
            throw new PlatformException(ex.ExpandExceptionMessage());
        }

        public static string ExpandExceptionMessage(this Exception ex)
        {
            var builder = new StringBuilder();

            var separator = Environment.NewLine;
            var exception = ex;
            var depthLevel = 0;
            const int maxDepthLevel = 5;
            while (exception != null && depthLevel++ < maxDepthLevel)
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

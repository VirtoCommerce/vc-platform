using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ThrowFaultException(this Exception ex)
        {
            throw new FaultException(ex.ExpandExceptionMessage());
        }

        public static string ExpandExceptionMessage(this Exception ex)
        {
            var builder = new StringBuilder();

            string separator = Environment.NewLine;
            var exception = ex;

            while (exception != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(separator);
                }

                builder.Append(exception.Message);

                var dbEntityValidationException = exception as DbEntityValidationException;

                if (dbEntityValidationException != null)
                {
                    var validationErrors = dbEntityValidationException.EntityValidationErrors.SelectMany(x => x.ValidationErrors.Select(y => y.ErrorMessage));

                    foreach (var validationError in validationErrors)
                    {
                        builder.Append(separator);
                        builder.Append(validationError);
                    }
                }

                exception = exception.InnerException;
            }

            return builder.ToString();
        }
    }
}

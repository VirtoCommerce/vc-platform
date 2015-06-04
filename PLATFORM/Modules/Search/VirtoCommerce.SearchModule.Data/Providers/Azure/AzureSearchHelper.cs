using System;
using System.Text;
using RedDog.Search.Http;

namespace VirtoCommerce.SearchModule.Data.Providers.Azure
{
    public class AzureSearchHelper
    {
        public static DateTimeOffset ConvertToOffset(DateTime value)
        {
            if (value == DateTime.MaxValue)
            {
                return DateTimeOffset.MaxValue;
            }

            return new DateTimeOffset(value);
        }

        public static string Create(string field, string value)
        {
            return String.Format("{0} eq '{1}'", field, value);
        }

        public static string CombineFilters(string original, string target, bool and = true)
        {
            return String.Format("{0}{1}{2}", original.Length > 0 ?
                String.Format("{0} {1} ", original, and ? "and" : "or") : "", target);
        }

        public static void Combine(StringBuilder original, string target, bool and = true)
        {
            if (original.Length > 0)
                original.AppendFormat(" {0} ", and ? "AND" : "OR");

            original.Append(target);
        }

        public static string FormatSearchException(IApiResponse response)
        {
            return String.Format(
                "StatusCode: {0}; Error Code: {1}; Error Message: {2}",
                response.StatusCode,
                response.Error.Code,
                response.Error.Message);
        }
    }
}

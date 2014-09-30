using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Search.Providers.Azure
{
    public static class StringBuilderExtensions
    {
        public static void Filter(this StringBuilder builder, string field, string value, bool and = true)
        {
            if (builder.Length > 0) builder.AppendFormat(" {0} ", and ? "and" : "or");

            builder.AppendFormat("{0} eq '{1}'", field, value);
        }

        public static void Filter(this StringBuilder builder, string field, string[] values, bool and = true)
        {
            if (builder.Length > 0) builder.AppendFormat(" {0} ", and ? "and" : "or");

            var index = 0;
            builder.AppendFormat("(");
            foreach (var val in values)
            {
                if (index > 0)
                {
                    builder.AppendFormat(" {0} ", "or");
                }

                builder.AppendFormat("{0} eq '{1}'", field, val);
                index++;
            }
            builder.AppendFormat(")");
        }

        public static void Filter(this StringBuilder builder, string field, DateTime value, string op, bool and = true)
        {
            if (builder.Length > 0) builder.AppendFormat(" {0} ", and ? "and" : "or");

            builder.AppendFormat("(");
            builder.AppendFormat("{0} {1} {2}", field, op, AzureSearchHelper.ConvertToOffset(value).ToString("u").Replace(" ", "T"));
            builder.AppendFormat(")");
        }

        public static void FilterContains(this StringBuilder builder, string field, string value, bool and = true)
        {
            if (builder.Length > 0) builder.AppendFormat(" {0} ", and ? "and" : "or");

            builder.AppendFormat("{0}/any(t: t eq '{1}')", field, value);
        }

        public static void FilterContains(this StringBuilder builder, string field, string[] values, bool and = true)
        {
            if (values.Length == 0) return;
            
            if (builder.Length > 0) builder.AppendFormat(" {0} ", and ? "and" : "or");

            var index = 0;
            builder.AppendFormat("{0}/any(t:", field);
            foreach (var val in values)
            {
                if (index > 0)
                {
                    builder.AppendFormat(" {0} ", "or");
                }

                builder.AppendFormat("t eq '{0}'", val);
                index++;
            }

            builder.AppendFormat(")");
        }

        public static void Query(this StringBuilder builder, string value, bool and = true)
        {
            if (builder.Length > 0) builder.AppendFormat("{0}", and ? "+" : "|");
            builder.AppendFormat("{0}", value);
        }

        public static void Query(this StringBuilder builder, string field, string[] values, bool and = true)
        {
            if (values.Length == 0) return;

            if (builder.Length > 0) builder.AppendFormat("{0}", and ? "+" : "|");

            var index = 0;
            builder.AppendFormat("(");
            foreach (var val in values)
            {
                if (index > 0)
                {
                    builder.AppendFormat("{0}", "|");
                }

                builder.AppendFormat("{0}:{1}", field, val);
                index++;
            }

            builder.AppendFormat(")");
        }
    }
}

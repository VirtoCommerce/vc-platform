using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public static class StringBuilderExtension
    {
        public static StringBuilder AppendProperties(this StringBuilder stringBuilder, List<Tuple<string, string>> properties, int padLeft)
        {
            int widthMax = properties.Select(p => p.Item1.Length).Max();
            int columnWidth = widthMax + 2 + padLeft;
            foreach (var row in properties)
            {
                stringBuilder.AppendAlignLeft(row.Item1, padLeft, columnWidth).Append(row.Item2);
            }
            return stringBuilder;
        }

        public static StringBuilder AppendColumn(this StringBuilder builder, params Tuple<string, int>[] pairs)
        {
            foreach (var p in pairs)
            {
                builder.AppendAlignCenter(p.Item1, p.Item2);
            }
            return builder;
        }

        public static StringBuilder AppendAlignCenter(this StringBuilder builder, string text, int width)
        {

            if (String.IsNullOrEmpty(text))
            {
                builder.Append(new string(' ', width));
            }
            else
            {
                int diff = width - text.Length, padLeft = 0, padRight = 0;
                if (diff > 0)
                {
                    padLeft = diff / 2;
                    padRight = diff - padLeft;
                }
                builder.Append(new string(' ', padLeft)).Append(text).Append(new string(' ', padRight));
            }
            return builder;
        }

        private static StringBuilder AppendAlignLeft(this StringBuilder builder, string text, int padLeft, int maxWidth)
        {
            if (String.IsNullOrEmpty(text))
            {
                builder.Append(new string(' ', maxWidth));
            }
            else
            {
                int diff = maxWidth - text.Length - padLeft, padRight = 0;
                if (diff > 0)
                {
                    padRight = diff - padLeft;
                }
                builder.Append(new string(' ', padLeft)).Append(text).Append(new string(' ', padRight));
            }
            return builder;
        }

        public static StringBuilder AppendXml<T>(this StringBuilder sb, T t)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter(sb);
                serializer.Serialize(stringWriter, t);
                stringWriter.Close();
            }
            catch (Exception ex)
            {
                sb.Append("Unsiccessful serialization of type (")
                  .Append(typeof(T).FullName)
                  .AppendLine(") instance!").Append("Details: ").AppendException(ex);
            }
            return sb;
        }

        private static void AppendExceptionGeneralProperties(this StringBuilder sb, Exception ex)
        {
            sb.AppendFormat("'{0}'   ({1})", ex.Message, ex.GetType().FullName).Append(Environment.NewLine);
            sb.Append(ex.StackTrace).Append(Environment.NewLine);
            if (ex.Data.Count > 0)
            {
                sb.Append("***** Exception.Data collection").Append(Environment.NewLine);
                foreach (var key in ex.Data.Keys)
                {
                    sb.AppendFormat("[Key {0}] {1}" + Environment.NewLine, key, ex.Data[key]);
                }
            }
        }

        public static StringBuilder AppendException(this StringBuilder sb, Exception ex)
        {
            return AppendException(sb, ex, null);

        }

        public static StringBuilder AppendException(
            this StringBuilder sb,
            Exception ex,
            Action<StringBuilder, Exception> appendSpecificProperties)
        {
            sb.Append("EXCEPTION CATCHED: ").AppendExceptionGeneralProperties(ex);
            if (appendSpecificProperties != null)
                appendSpecificProperties(sb, ex);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                sb.Append("INNER EXCEPTION: ").AppendExceptionGeneralProperties(ex);
                if (appendSpecificProperties != null)
                    appendSpecificProperties(sb, ex);
            }
            return sb;
        }
    }

}

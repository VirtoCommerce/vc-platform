using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public static class SerialisationToString
    {
        public static string FormatCanon(this List<string> list, string delimiter)
        {
            var @value = "";
            bool first = true;
            foreach (var s in list)
            {
                if (first)
                    first = false;
                else
                    @value += delimiter;
                @value += s;
            }
            return @value;
        }

        public static string Format(this Exception ex)
        {
            var builder = new StringBuilder();
            builder.AppendException(ex);
            return builder.ToString();
        }

        public static string FormatInformationSize(this double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static string FormatInformationSize(this long len)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static List<string> BreakLine(this string text, int width)
        {
            int start = 0, end;
            var lines = new List<string>();
            //text = Regex.Replace(text, @"\s", " ").Trim();

            while ((end = start + width) < text.Length)
            {
                while (text[end] != ' ' && end > start)
                    end -= 1;

                if (end == start)
                    end = start + width;

                lines.Add(text.Substring(start, end - start));
                start = end + 1;
            }

            if (start < text.Length)
                lines.Add(text.Substring(start));

            return lines;
        }

        public static string SerializeTable(List<int> columnwidth, List<string> headers, List<List<string>> table)
        {

            if ((headers != null && columnwidth.Count != headers.Count) || (table.Count > 0 && columnwidth.Count != table[0].Count))
            {
                throw new ApplicationException("Not consistent parameters");
            }
            var sb = new StringBuilder();
            if (headers != null)
            {
                for (var c = 0; c < columnwidth.Count; c++)
                {
                    sb.AppendColumn(new Tuple<string, int>(headers[c].FormatCompact(columnwidth[c]), columnwidth[c]));
                }
                sb.AppendLine();
            }
            foreach (var row in table)
            {
                for (var c = 0; c < columnwidth.Count; c++)
                {
                    sb.AppendColumn(new Tuple<string, int>(row[c].FormatCompact(columnwidth[c]), columnwidth[c]));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static string SerializeProperties(List<Tuple<string, string>> properties, int padLeft)
        {
            var sb = new StringBuilder();
            sb.AppendProperties(properties, padLeft);
            return sb.ToString();
        }

        public static string FormatCanon(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.FormatCanon() : "n/e";
        }

        public static string FormatCanon(this DateTime dt)
        {
            return dt.ToString("yy.MM.dd HH:mm");
        }

        public static string FormatCompact(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.FormatCompact() : "n/e";
        }

        public static string FormatCompact(this DateTime dt)
        {
            return dt.ToString("yyMMdd HHmm");
        }

        public static string FormatCanon(this string str)
        {
            return String.IsNullOrEmpty(str) ? "" : str;
        }

        public static string FormatCompact(this string str, int width)
        {
            if (String.IsNullOrEmpty(str))
                return "";
            else if (str.Length <= width)
                return str;
            else
                try
                {
                    return str.Substring(0, width - 1) + "…";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }
    }

}

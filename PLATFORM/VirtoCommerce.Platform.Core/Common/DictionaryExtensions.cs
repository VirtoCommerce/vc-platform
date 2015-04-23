using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class DictionaryExtensions
    {
        public static string ToString(this IEnumerable<KeyValuePair<string, string>> pairs, string pairSeparator, string valueSeparator, params string[] excludeNames)
        {
            var builder = new StringBuilder();

            foreach (var pair in pairs.Where(pair => !excludeNames.Contains(pair.Key, StringComparer.OrdinalIgnoreCase)))
            {
                if (builder.Length > 0)
                {
                    builder.Append(pairSeparator);
                }
                builder.Append(pair.Key);
                builder.Append(valueSeparator);
                builder.Append(pair.Value);
            }

            return builder.ToString();
        }
    }
}

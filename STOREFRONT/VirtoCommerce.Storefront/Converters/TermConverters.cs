using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Converters
{
    public static class TermConverters
    {
        public static List<string> ToStrings(this Term[] terms)
        {
            List<string> result = null;

            if (terms != null && terms.Any())
            {
                result = terms
                    .OrderBy(t => t.Name)
                    .GroupBy(t => t.Name, t => t, StringComparer.OrdinalIgnoreCase)
                    .Select(
                        g =>
                            string.Join(":", g.Key,
                                string.Join(",",
                                    g.Select(t => t.Value)
                                        .Distinct(StringComparer.OrdinalIgnoreCase)
                                        .OrderBy(v => v))))
                    .ToList();
            }

            return result;
        }
    }
}

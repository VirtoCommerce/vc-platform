using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Data.Services
{
    public static class SearchFilterExtensions
    {
        public static FacetLabel[] GetLabels(this ISearchFilter filter)
        {
            FacetLabel[] result = null;

            var attributeFilter = filter as AttributeFilter;
            if (attributeFilter != null)
            {
                if (attributeFilter.DisplayNames != null)
                {
                    result = attributeFilter.DisplayNames.Select(d => new FacetLabel { Language = d.Language, Label = d.Name }).ToArray();
                }
            }

            return result;
        }

        public static ISearchFilterValue[] GetValues(this ISearchFilter filter)
        {
            var attributeFilter = filter as AttributeFilter;
            if (attributeFilter != null)
            {
                return attributeFilter.Values != null ? attributeFilter.Values.OfType<ISearchFilterValue>().ToArray() : null;
            }

            var rangeFilter = filter as RangeFilter;
            if (rangeFilter != null)
            {
                return rangeFilter.Values != null ? rangeFilter.Values.OfType<ISearchFilterValue>().ToArray() : null;
            }

            var priceRangeFilter = filter as PriceRangeFilter;
            if (priceRangeFilter != null)
            {
                return priceRangeFilter.Values != null ? priceRangeFilter.Values.OfType<ISearchFilterValue>().ToArray() : null;
            }

            var categoryFilter = filter as CategoryFilter;
            if (categoryFilter != null)
            {
                return categoryFilter.Values != null ? categoryFilter.Values.OfType<ISearchFilterValue>().ToArray() : null;
            }

            return null;
        }

        public static FacetLabel[] GetValueLabels(this IEnumerable<ISearchFilterValue> values)
        {
            var result = values
                .SelectMany(GetValueLabels)
                .Where(l => !string.IsNullOrEmpty(l.Language) && !string.IsNullOrEmpty(l.Label))
                .GroupBy(v => v.Language, StringComparer.OrdinalIgnoreCase)
                .SelectMany(g => g
                    .GroupBy(g2 => g2.Label, StringComparer.OrdinalIgnoreCase)
                    .Select(g2 => g2.FirstOrDefault()))
            .OrderBy(v => v.Language)
            .ThenBy(v => v.Label)
            .ToArray();

            return result.Any() ? result : null;
        }


        private static List<FacetLabel> GetValueLabels(this ISearchFilterValue value)
        {
            var result = new List<FacetLabel>();

            var attributeFilterValue = value as AttributeFilterValue;
            if (attributeFilterValue != null)
            {
                result.Add(new FacetLabel { Language = attributeFilterValue.Language, Label = attributeFilterValue.Value });
            }

            var rangeFilterValue = value as RangeFilterValue;
            if (rangeFilterValue != null)
            {
                if (rangeFilterValue.Displays != null)
                {
                    var labels = rangeFilterValue.Displays
                        .Select(d => new FacetLabel { Language = d.Language, Label = d.Value })
                        .ToArray();
                    result.AddRange(labels);
                }
            }

            return result;
        }
    }
}

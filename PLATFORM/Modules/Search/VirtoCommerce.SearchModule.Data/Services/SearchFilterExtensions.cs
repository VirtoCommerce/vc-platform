using System;
using System.Linq;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Data.Services
{
    public static class SearchFilterExtensions
    {
        public static string GetDisplayName(this ISearchFilter filter, string locale, string localeShort)
        {
            var result = filter.Key;

            var attributeFilter = filter as AttributeFilter;
            if (attributeFilter != null)
            {
                if (attributeFilter.DisplayNames != null)
                {
                    var displayName = attributeFilter.GetDisplayName(locale);

                    if (string.IsNullOrEmpty(displayName) && localeShort != null)
                    {
                        displayName = attributeFilter.GetDisplayName(localeShort);
                    }

                    if (!string.IsNullOrEmpty(displayName))
                    {
                        result = displayName;
                    }
                }
            }

            return result;
        }

        public static string GetDisplayName(this AttributeFilter attributeFilter, string locale)
        {
            string result = null;

            if (attributeFilter.DisplayNames != null)
            {
                result = attributeFilter.DisplayNames
                    .Where(d => d.Language.Equals(locale, StringComparison.OrdinalIgnoreCase))
                    .Select(d => d.Name)
                    .FirstOrDefault();
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
                return rangeFilter.Values.OfType<ISearchFilterValue>().ToArray();
            }

            var priceRangeFilter = filter as PriceRangeFilter;
            if (priceRangeFilter != null)
            {
                return priceRangeFilter.Values.OfType<ISearchFilterValue>().ToArray();
            }

            var categoryFilter = filter as CategoryFilter;
            if (categoryFilter != null)
            {
                return categoryFilter.Values.OfType<ISearchFilterValue>().ToArray();
            }

            return null;
        }

        public static string GetDisplayValue(this ISearchFilterValue value, string locale, string localeShort)
        {
            var attributeFilterValue = value as AttributeFilterValue;
            if (attributeFilterValue != null)
            {
                return attributeFilterValue.Value;
            }

            var rangeFilterValue = value as RangeFilterValue;
            if (rangeFilterValue != null)
            {
                if (rangeFilterValue.Displays != null)
                {
                    var displayValue = rangeFilterValue.GetDisplayValue(locale);

                    if (string.IsNullOrEmpty(displayValue) && localeShort != null)
                    {
                        displayValue = rangeFilterValue.GetDisplayValue(localeShort);
                    }

                    if (!string.IsNullOrEmpty(displayValue))
                    {
                        return displayValue;
                    }
                }

                return rangeFilterValue.Id;
            }

            var categoryFilterValue = value as CategoryFilterValue;
            if (categoryFilterValue != null)
            {
                return categoryFilterValue.Name;
            }

            return string.Empty;
        }

        public static string GetDisplayValue(this RangeFilterValue rangeFilterValue, string locale)
        {
            string result = null;

            if (rangeFilterValue.Displays != null)
            {
                result = rangeFilterValue.Displays
                    .Where(d => d.Language.Equals(locale, StringComparison.OrdinalIgnoreCase))
                    .Select(d => d.Value)
                    .FirstOrDefault();
            }

            return result;
        }
    }
}

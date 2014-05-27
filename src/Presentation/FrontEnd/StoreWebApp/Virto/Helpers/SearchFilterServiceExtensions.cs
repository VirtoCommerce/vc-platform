using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Extensions;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Facets;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{

    /// <summary>
    /// Class SearchHelperExtensions.
    /// </summary>
    public static class SearchFilterServiceExtensions
    {
        /// <summary>
        /// Converts the specified filter to filter model.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="filter">The search filter.</param>
        /// <returns>Filter model</returns>
        public static FilterModel Convert(this ISearchFilterService helper, ISearchFilter filter)
        {
            var model = new FilterModel();

            if (filter is AttributeFilter)
            {
                var prop = filter as AttributeFilter;
                model.Key = prop.Key;
                model.Name = ClientContext.Clients.CreateCatalogClient().GetPropertyName(prop.Key);
                model.Name = string.IsNullOrEmpty(model.Name) ? model.Key : model.Name;
                return model;
            }
            if (filter is RangeFilter)
            {
                var prop = filter as RangeFilter;
                model.Key = prop.Key;
                model.Name = ClientContext.Clients.CreateCatalogClient().GetPropertyName(prop.Key);
                model.Name = string.IsNullOrEmpty(model.Name) ? model.Key : model.Name;
                return model;
            }
            if (filter is PriceRangeFilter)
            {
                var prop = filter as PriceRangeFilter;
                model.Key = prop.Key;
                model.Name = "Price";
                return model;
            }
            if (filter is CategoryFilter)
            {
                var prop = filter as CategoryFilter;
                model.Key = prop.Key;
                model.Name = "Category";
                return model;
            }

            return null;
        }

        /// <summary>
        /// Converts the specified filter value to facet model.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="val">The search filter value.</param>
        /// <returns>facet model</returns>
        public static FacetModel Convert(this ISearchFilterService helper, ISearchFilterValue val)
        {
            var model = new FacetModel();

            if (val is AttributeFilterValue)
            {
                var v = val as AttributeFilterValue;
                model.Key = v.Id;
                model.Name = v.Value;
                return model;
            }
            if (val is CategoryFilterValue)
            {
                var v = val as CategoryFilterValue;
                model.Key = v.Id;
                model.Name = v.Name;
                return model;
            }
            if (val is RangeFilterValue)
            {
                var v = val as RangeFilterValue;
                model.Key = v.Id;

                var name = String.Empty;
                if (v.Displays != null)
                {
                    var disp = (from d in v.Displays where d.Language == "en" select d).SingleOrDefault();
                    if (disp != null)
                    {
                        name = disp.Value;
                    }
                }

                model.Name = name;
                return model;
            }

            return null;
        }

        /// <summary>
        /// Converts the specified facet groups into filter model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="groups">The groups.</param>
        /// <returns>
        /// FilterModel[][].
        /// </returns>
        public static FilterModel[] Convert(this ISearchFilterService helper, FacetGroup[] groups)
        {
            var list = new List<FilterModel>();
            if (groups != null)
            {
                list.AddRange(groups.Select(x => Convert(helper, x)));
            }

            return list.ToArray();
        }

        /// <summary>
        /// Converts the specified facet group.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="group">The facet group.</param>
        /// <returns>
        /// facet group
        /// </returns>
        public static FilterModel Convert(this ISearchFilterService helper, FacetGroup group)
        {
            return new FilterModel
            {
                Key = @group.FieldName,
                Name = GetDescriptionFromFilter(@group.FieldName),
                Facets = @group.Facets.Select(x => Convert(helper, x)).ToArray()
            };
        }

        /// <summary>
        /// Converts the specified facet to facet model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="facet">The facet.</param>
        /// <returns>
        /// facet model
        /// </returns>
        public static FacetModel Convert(this ISearchFilterService helper, Facet facet)
        {
            return new FacetModel
            {
                Key = facet.Key,
                Name = GetNameFromFilterValue(helper, facet),
                Count = facet.Count
            };
        }

        public static ISearchFilter Convert(this ISearchFilterService helper, ISearchFilter filter, string[] keys)
        {
            // get values that we have filters set for
            var values = from v in filter.GetValues() where keys.Contains(v.Id) select v;

            var attributeFilter = filter as AttributeFilter;
            if (attributeFilter != null)
            {
                var newFilter = new AttributeFilter();
                newFilter.InjectFrom(filter);
                newFilter.Values = values.OfType<AttributeFilterValue>().ToArray();
                return newFilter;
            }

            var rangeFilter = filter as RangeFilter;
            if (rangeFilter != null)
            {
                var newFilter = new RangeFilter();
                newFilter.InjectFrom(filter);

                newFilter.Values = values.OfType<RangeFilterValue>().ToArray();
                return newFilter;
            }

            var priceRangeFilter = filter as PriceRangeFilter;
            if (priceRangeFilter != null)
            {
                var newFilter = new PriceRangeFilter();
                newFilter.InjectFrom(filter);

                newFilter.Values = values.OfType<RangeFilterValue>().ToArray();
                return newFilter;
            }

            var categoryFilter = filter as CategoryFilter;
            if (categoryFilter != null)
            {
                var newFilter = new CategoryFilter();
                newFilter.InjectFrom(filter);
                newFilter.Values = values.OfType<CategoryFilterValue>().ToArray();
                return newFilter;
            }

            return null;
        }


        #region Private Helpers

        /// <summary>
        /// Gets the description from filter.
        /// </summary>
        /// <param name="key">The key of the group.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        private static string GetDescriptionFromFilter(string key)
        {
            if (key.Equals("__outline")) return "Subcategory";

            var name = ClientContext.Clients.CreateCatalogClient().GetPropertyName(key);
            //return key.Equals("price", StringComparison.OrdinalIgnoreCase) ? "Price".Localize() : !string.IsNullOrEmpty(name) ? name : key;
            return !string.IsNullOrEmpty(name) ? name : key;
        }

        /// <summary>
        /// Gets the description from filter value.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="facet">The facet.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        private static string GetNameFromFilterValue(ISearchFilterService helper, Facet facet)
        {
            var key = facet.Group.FieldName;
            var id = facet.Key;

            var name = facet.Name;

            var d = (from f in helper.Filters
                     where f.Key.Equals(key, StringComparison.OrdinalIgnoreCase) &&
                         (f as PriceRangeFilter == null ||
                         ((PriceRangeFilter)f).Currency.Equals(ClientContext.Clients.CreateCatalogClient().CustomerSession.Currency, StringComparison.OrdinalIgnoreCase))
                     select f).SingleOrDefault();

            if (d != null)
            {
                var val = (from v in d.GetValues() where v.Id.Equals(id, StringComparison.OrdinalIgnoreCase) select v).SingleOrDefault();
                if (val != null)
                {
                    name = Convert(helper, val).Name;
                }
            }

            return name;
        }


        #endregion
    }
}
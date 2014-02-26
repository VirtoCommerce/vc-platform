using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Facets;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{
	/// <summary>
    /// Class SearchHelperExtensions.
	/// </summary>
	public static class SearchHelperExtensions
	{
	    /// <summary>
	    /// Converts the specified filter to filter model.
	    /// </summary>
	    /// <param name="helper"></param>
	    /// <param name="filter">The search filter.</param>
	    /// <returns>Filter model</returns>
	    public static FilterModel Convert(this SearchHelper helper, ISearchFilter filter)
		{
			var model = new FilterModel();

			if (filter is AttributeFilter)
			{
				var prop = filter as AttributeFilter;
				model.Key = prop.Key;
				model.Name = helper.CatalogClient.GetPropertyName(prop.Key);
				model.Name = string.IsNullOrEmpty(model.Name) ? model.Key : model.Name;
				return model;
			}
			if (filter is RangeFilter)
			{
				var prop = filter as RangeFilter;
				model.Key = prop.Key;
				model.Name = helper.CatalogClient.GetPropertyName(prop.Key);
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

			return null;
		}

	    /// <summary>
	    /// Converts the specified filter value to facet model.
	    /// </summary>
	    /// <param name="helper"></param>
	    /// <param name="val">The search filter value.</param>
	    /// <returns>facet model</returns>
	    public static FacetModel Convert(this SearchHelper helper, ISearchFilterValue val)
		{
			var model = new FacetModel();

			if (val is AttributeFilterValue)
			{
				var v = val as AttributeFilterValue;
				model.Key = v.Id;
				model.Name = v.Value;
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
	    public static FilterModel[] Convert(this SearchHelper helper, FacetGroup[] groups)
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
	    public static FilterModel Convert(this SearchHelper helper, FacetGroup group)
		{
			return new FilterModel
			{
				Key = @group.FieldName,
				Name = GetDescriptionFromFilter(helper, @group.FieldName),
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
	    public static FacetModel Convert(this SearchHelper helper, Facet facet)
		{
			return new FacetModel
			{
				Key = facet.Key,
                Name = GetDescriptionFromFilterValue(helper, facet.Group.FieldName, facet.Key),
				Count = facet.Count
			};
		}

		#region Private Helpers

        /// <summary>
        /// Gets the description from filter.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// System.String.
        /// </returns>
		private static string GetDescriptionFromFilter(SearchHelper helper, string key)
		{
            var name = helper.CatalogClient.GetPropertyName(key);
			return key.Equals("price", StringComparison.OrdinalIgnoreCase) ? "Price".Localize() : !string.IsNullOrEmpty(name) ? name : key;
		}

        /// <summary>
        /// Gets the description from filter value.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="key">The key.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        private static string GetDescriptionFromFilterValue(SearchHelper helper, string key, string id)
		{
			var desc = String.Empty;

            var d = (from f in helper.Filters where f.Key.Equals(key, StringComparison.OrdinalIgnoreCase) && 
                         (f as PriceRangeFilter == null || ((PriceRangeFilter)f).Currency.Equals(helper.CatalogClient.CustomerSession.Currency)) select f).SingleOrDefault();
			if (d != null)
			{
                var val = (from v in helper.GetFilterValues(d) where v.Id.Equals(id, StringComparison.OrdinalIgnoreCase) select v).SingleOrDefault();
				if (val != null)
				{
                    desc = Convert(helper, val).Name;
				}
			}

			return desc;
		}

		#endregion
	}
}
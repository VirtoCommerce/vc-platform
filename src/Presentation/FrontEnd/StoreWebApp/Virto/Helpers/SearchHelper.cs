using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Facets;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Web.Client.Globalization;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{
	/// <summary>
	/// Class SearchHelper.
	/// </summary>
	public class SearchHelper
	{
		#region Static initialization

		#endregion

		#region Private Fields

		/// <summary>
		/// The _store
		/// </summary>
		private readonly Store _store;
		/// <summary>
		/// The _catalog client
		/// </summary>
		private CatalogClient _catalogClient;
		/// <summary>
		/// The _filters
		/// </summary>
		private ISearchFilter[] _filters;

		#endregion

		#region Contructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SearchHelper"/> class.
		/// </summary>
		/// <param name="store">The store.</param>
		public SearchHelper(Store store)
		{
			_store = store;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the search filters from store settings
		/// </summary>
		/// <value>The filters.</value>
		public ISearchFilter[] Filters
		{
			get { return _filters ?? (_filters = GetStoreAllFilters(_store)); }
		}

		/// <summary>
		/// Gets the catalog client.
		/// </summary>
		/// <value>The catalog client.</value>
		public CatalogClient CatalogClient
		{
			get { return _catalogClient ?? (_catalogClient = DependencyResolver.Current.GetService<CatalogClient>()); }
		}

		#endregion

		#region Search Helpers

		/// <summary>
		/// Gets the store browse filters.
		/// </summary>
		/// <param name="store">The store.</param>
		/// <returns>Filtered browsing</returns>
		public static FilteredBrowsing GetStoreBrowseFilters(Store store)
		{
			var filter = (from s in store.Settings where s.Name == "FilteredBrowsing" select s.LongTextValue).FirstOrDefault();
			if (!string.IsNullOrEmpty(filter))
			{
				var filterString = filter;
				var serializer = new XmlSerializer(typeof(FilteredBrowsing));
				TextReader reader = new StringReader(filterString);
				var browsing = serializer.Deserialize(reader) as FilteredBrowsing;
				return browsing;
			}

			return null;
		}

        /// <summary>
        /// Gets the filter values.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable{ISearchFilterValue}.</returns>
		public IEnumerable<ISearchFilterValue> GetFilterValues(ISearchFilter filter)
		{

		    var attributeFilter = filter as AttributeFilter;
		    if (attributeFilter != null)
			{
				return attributeFilter.Values;
			}

		    var rangeFilter = filter as RangeFilter;
		    if (rangeFilter != null)
		    {
		        return  rangeFilter.Values;
		    }

		    var priceRangeFilter = filter as PriceRangeFilter;
		    if (priceRangeFilter != null)
		    {
		        return priceRangeFilter.Values;
		    }

		    return null;
		}

		/// <summary>
		/// Converts the specified filter to filter model.
		/// </summary>
		/// <param name="filter">The search filter.</param>
		/// <returns>Filter model</returns>
		public FilterModel Convert(ISearchFilter filter)
		{
			var model = new FilterModel();

			if (filter is AttributeFilter)
			{
				var prop = filter as AttributeFilter;
				model.Key = prop.Key;
				model.Name = CatalogClient.GetPropertyName(prop.Key);
				model.Name = string.IsNullOrEmpty(model.Name) ? model.Key : model.Name;
				return model;
			}
			if (filter is RangeFilter)
			{
				var prop = filter as RangeFilter;
				model.Key = prop.Key;
				model.Name = CatalogClient.GetPropertyName(prop.Key);
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
		/// <param name="val">The search filter value.</param>
		/// <returns>facet model</returns>
		public FacetModel Convert(ISearchFilterValue val)
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
		/// <param name="groups">The groups.</param>
		/// <returns>FilterModel[][].</returns>
		public FilterModel[] Convert(FacetGroup[] groups)
		{
			var list = new List<FilterModel>();
			if (groups != null)
			{
				list.AddRange(groups.Select(Convert));
			}

			return list.ToArray();
		}

		/// <summary>
		/// Converts the specified facet group.
		/// </summary>
		/// <param name="group">The facet group.</param>
		/// <returns>facet group</returns>
		public FilterModel Convert(FacetGroup group)
		{
			return new FilterModel
			{
				Key = @group.FieldName,
				Name = GetDescriptionFromFilter(@group.FieldName),
				Facets = @group.Facets.Select(Convert).ToArray()
			};
		}

		/// <summary>
		/// Converts the specified facet to facet model.
		/// </summary>
		/// <param name="facet">The facet.</param>
		/// <returns>facet model</returns>
		public FacetModel Convert(Facet facet)
		{
			return new FacetModel
			{
				Key = facet.Key,
				Name = GetDescriptionFromFilterValue(facet.Group.FieldName, facet.Key),
				Count = facet.Count
			};
		}

		#endregion

		#region Private Helpers

		/// <summary>
		/// Gets the description from filter.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>System.String.</returns>
		private string GetDescriptionFromFilter(string key)
		{
			var name = CatalogClient.GetPropertyName(key);
			return key.Equals("price", StringComparison.OrdinalIgnoreCase) ? "Price".Localize() : !string.IsNullOrEmpty(name) ? name : key;
		}

		/// <summary>
		/// Gets the description from filter value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="id">The identifier.</param>
		/// <returns>System.String.</returns>
		private string GetDescriptionFromFilterValue(string key, string id)
		{
			var desc = String.Empty;

			var d = (from f in Filters where f.Key.Equals(key, StringComparison.OrdinalIgnoreCase) select f).SingleOrDefault();
			if (d != null)
			{
				var val = (from v in GetFilterValues(d) where v.Id.Equals(id, StringComparison.OrdinalIgnoreCase) select v).SingleOrDefault();
				if (val != null)
				{
					desc = Convert(val).Name;
				}
			}

			return desc;
		}

		/// <summary>
		/// Gets the store all filters.
		/// </summary>
		/// <param name="store">The store.</param>
		/// <returns>ISearchFilter[][].</returns>
		private ISearchFilter[] GetStoreAllFilters(Store store)
		{
			var filters = new List<ISearchFilter>();
			var browsing = GetStoreBrowseFilters(store);
			if (browsing != null)
			{
				if (browsing.Attributes != null)
				{
					filters.AddRange(browsing.Attributes);
				}
				if (browsing.AttributeRanges != null)
				{
					filters.AddRange(browsing.AttributeRanges);
				}
				if (browsing.Prices != null)
				{
					filters.AddRange(browsing.Prices);
				}
			}

			return filters.ToArray();
		}

		#endregion
	}
}
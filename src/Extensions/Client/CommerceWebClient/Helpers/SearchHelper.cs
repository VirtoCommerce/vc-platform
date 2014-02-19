using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Foundation.Stores.Model;

namespace VirtoCommerce.Web.Client.Helpers
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

		#endregion

		#region Private Helpers

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
namespace VirtoCommerce.Domain.Search.Model
{
    public interface ISearchCriteria
    {
        string DocumentType { get; }
        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <value>The cache key.</value>
        string CacheKey { get; }

        /// <summary>
        /// Gets or sets the starting record.
        /// </summary>
        /// <value>The starting record.</value>
        int StartingRecord { get; set; }

        /// <summary>
        /// Gets or sets the records to retrieve.
        /// </summary>
        /// <value>The records to retrieve.</value>
        int RecordsToRetrieve { get; set; }

        /// <summary>
        /// Gets the sorts.
        /// </summary>
        /// <value>The sorts.</value>
        SearchSort Sort { get; set; }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <value>The locale.</value>
        string Locale { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        string Currency { get; set; }

        /// <summary>
        /// Gets the key field.
        /// </summary>
        /// <value>The key field.</value>
        string KeyField { get; }

		/// <summary>
		/// Gets the outline field.
		/// </summary>
		/// <value>The outline field.</value>
		string OutlineField { get; }

        /// <summary>
        /// Gets the reviews total field.
        /// </summary>
        /// <value>
        /// The reviews total field.
        /// </value>
        string ReviewsTotalField { get; }

        /// <summary>
        /// Gets the reviews average field.
        /// </summary>
        /// <value>
        /// The reviews average field.
        /// </value>
        string ReviewsAverageField { get; }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <value>The filters.</value>
        ISearchFilter[] Filters { get; }

        /// <summary>
        /// Adds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        void Add(ISearchFilter filter);

        ISearchFilter[] CurrentFilters { get; }

        /// <summary>
        /// Applies the specified filter.
        /// </summary>
        /// <param name="field">The field.</param>
        void Apply(ISearchFilter filter);
    }
}

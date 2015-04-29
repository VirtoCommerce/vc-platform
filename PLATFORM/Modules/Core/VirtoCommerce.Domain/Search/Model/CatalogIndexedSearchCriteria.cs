using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Foundation.Search;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace VirtoCommerce.Domain.Search
{
    public class CatalogIndexedSearchCriteria : KeywordSearchCriteria
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogItemSearchCriteria"/> class.
		/// </summary>
		/// <param name="documentType">Type of the document.</param>
		public CatalogIndexedSearchCriteria(string documentType)
			: base(documentType)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogItemSearchCriteria"/> class.
		/// </summary>
		public CatalogIndexedSearchCriteria()
			: base("catalogitem")
		{
		}

        /// <summary>
        /// Gets the default sort order.
        /// </summary>
        /// <value>The default sort order.</value>
        public static SearchSort DefaultSortOrder { get { return new SearchSort("__sort", false); } }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        public override SearchSort Sort
        {
            get
            {
                return base.Sort;
            }
            set
            {
                base.Sort = value;
            }
        }

        private bool _isFuzzySearch;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fuzzy search.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fuzzy search; otherwise, <c>false</c>.
        /// </value>
        public bool IsFuzzySearch
        {
            get { return _isFuzzySearch; }
            set { ChangeState(); _isFuzzySearch = value; }
        }

        private float _fuzzyMinSimilarity = 0.7f;

        /// <summary>
        /// Gets or sets the fuzzy min similarity.
        /// </summary>
        /// <value>The fuzzy min similarity.</value>
        public float FuzzyMinSimilarity
        {
            get { return _fuzzyMinSimilarity; }
            set { ChangeState(); _fuzzyMinSimilarity = value; }
        }

        private string _catalog;
        /// <summary>
        /// Gets or sets the indexes of the search.
        /// </summary>
        /// <value>
        /// The index of the search.
        /// </value>
        public virtual string Catalog
        {
            get { return _catalog; }
            set { ChangeState(); _catalog = value; }
        }

        private string[] _responseGroups;
        /// <summary>
        /// Gets or sets the response groups.
        /// </summary>
        /// <value>
        /// The response groups.
        /// </value>
        public virtual string[] ResponseGroups
        {
            get { return _responseGroups; }
            set { ChangeState(); _responseGroups = value; }
        }

        private StringCollection _outlines = new StringCollection();
        /// <summary>
        /// Gets or sets the outlines. Outline consists of "Category1/Category2".
        /// </summary>
        /// <example>Everything/digital-cameras</example>
        /// <value>The outlines.</value>
        public virtual StringCollection Outlines
        {
            get { return _outlines; }
            set { ChangeState(); _outlines = value; }
        }

   
        private string[] _pricelists;
        /// <summary>
        /// Gets or sets the price lists that should be considered for filtering.
        /// </summary>
        /// <value>
        /// The price lists.
        /// </value>
        public virtual string[] Pricelists
        {
            get { return _pricelists; }
            set { ChangeState(); _pricelists = value; }
        }

        private StringCollection _searchIndex = new StringCollection();

        /// <summary>
        /// Gets or sets the indexes of the search.
        /// </summary>
        /// <value>The index of the search.</value>
        public virtual StringCollection SearchIndex
        {
            get { return _searchIndex; }
            set { ChangeState(); _searchIndex = value; }
        }

        private StringCollection _classType = new StringCollection();

        /// <summary>
        /// Gets or sets the class types.
        /// </summary>
        /// <value>The class types.</value>
        public virtual StringCollection ClassTypes
        {
            get { return _classType; }
            set { ChangeState(); _classType = value; }
        }

        private DateTime _startDate = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the start date. The date must be in UTC format as that is format indexes are stored in.
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate
        {
            get { return _startDate; }
            set { ChangeState(); _startDate = value; }
        }

        private DateTime? _startDateFrom;

        /// <summary>
        /// Gets or sets the start date from filter. Used for filtering new products. The date must be in UTC format as that is format indexes are stored in.
        /// </summary>
        /// <value>The start date from.</value>
        public DateTime? StartDateFrom
        {
            get { return _startDateFrom; }
            set { ChangeState(); _startDateFrom = value; }
        }



        private DateTime? _endDate;

        /// <summary>
        /// Gets or sets the end date. The date must be in UTC format as that is format indexes are stored in.
        /// </summary>
        /// <value>The end date.</value>
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { ChangeState(); _endDate = value; }
        }

       

        /// <summary>
        /// Gets the cache key. Used to generate hash that will be used to store data in memory if needed.
        /// </summary>
        /// <value>The cache key.</value>
        public override string CacheKey
        {
            get
            {
                var key = new StringBuilder();

				key.Append("_rg" + ResponseGroups);
                key.Append("_ct" + Catalog);
                key.Append("_fs" + IsFuzzySearch.ToString());
                key.Append("_pl" + String.Join("-", Pricelists));
                key.Append("_st" + StartDate.ToString("s"));
                key.Append("_ed" + (EndDate.HasValue ? EndDate.Value.ToString("s") : ""));
                key.Append("_phr" + SearchPhrase);
                // Add active fields

                if (Outlines != null)
                {
                    foreach (var outline in Outlines)
                    {
                        key.Append("_out:" + outline);
                    }
                }

                if (SearchIndex != null)
                {
                    foreach (var search in SearchIndex)
                    {
                        key.Append("_in:" + search);
                    }
                }

                if (ClassTypes != null)
                {
                    foreach (var ct in ClassTypes)
                    {
                        key.Append("_ct:" + ct);
                    }
                }

                return base.CacheKey + key;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Foundation.Search;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Catalogs.Search
{
    [DataContract]
    public class CatalogItemSearchCriteria : KeywordSearchCriteria
    {
        /// <summary>
        /// Gets the default sort order.
        /// </summary>
        /// <value>The default sort order.</value>
        public static SearchSort DefaultSortOrder { get { return new SearchSort("__sort", false); } }

        /// <summary>
        /// Gets or sets the sort.
        /// </summary>
        /// <value>The sort.</value>
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public virtual StringCollection Outlines
        {
            get { return _outlines; }
            set { ChangeState(); _outlines = value; }
        }

        /*
        private List<ChildCategoryFilter> _childCategoryFilters = new List<ChildCategoryFilter>();
        /// <summary>
        /// Gets or sets the child category filters.
        /// </summary>
        /// <value>
        /// The child category filters.
        /// </value>
        [DataMember]
        public virtual List<ChildCategoryFilter> ChildCategoryFilters
        {
            get { return _childCategoryFilters; }
            set { ChangeState(); _childCategoryFilters = value; }
        }
         * */

        private string[] _pricelists;
        /// <summary>
        /// Gets or sets the price lists that should be considered for filtering.
        /// </summary>
        /// <value>
        /// The price lists.
        /// </value>
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { ChangeState(); _endDate = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemSearchCriteria"/> class.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        public CatalogItemSearchCriteria(string documentType)
            : base(documentType)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogItemSearchCriteria"/> class.
        /// </summary>
        public CatalogItemSearchCriteria()
            : base("catalogitem")
        {
        }

        /// <summary>
        /// Gets the cache key. Used to generate hash that will be used to store data in memory if needed.
        /// </summary>
        /// <value>The cache key.</value>
        [IgnoreDataMember]
        public override string CacheKey
        {
            get
            {
                var key = new StringBuilder();

                key.Append(CacheHelper.CreateCacheKey("_rg", ResponseGroups));
                key.Append(CacheHelper.CreateCacheKey("_ct", Catalog));
                key.Append(CacheHelper.CreateCacheKey("_fs", IsFuzzySearch.ToString()));
                key.Append(CacheHelper.CreateCacheKey("_pl", Pricelists));
                key.Append(CacheHelper.CreateCacheKey("_st", StartDate.ToString("s")));
                key.Append(CacheHelper.CreateCacheKey("_ed", EndDate.HasValue ? EndDate.Value.ToString("s") : ""));
                key.Append(CacheHelper.CreateCacheKey("_phr", SearchPhrase));
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
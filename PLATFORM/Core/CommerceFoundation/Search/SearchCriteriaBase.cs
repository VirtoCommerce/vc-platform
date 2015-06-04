using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Search.Schemas;

namespace VirtoCommerce.Foundation.Search
{

    [DataContract]
    [KnownType(typeof(AttributeFilter))]
    [KnownType(typeof(RangeFilter))]
    [KnownType(typeof(PriceRangeFilter))]
    [KnownType(typeof(CategoryFilter))]
    public abstract class SearchCriteriaBase : ISearchCriteria
    {
        string _documentType = String.Empty;
        /// <summary>
        /// Gets the scope.
        /// </summary>
        [DataMember]
        public virtual string DocumentType
        {
            get { return _documentType; }
            private set { _documentType = value; }
        }

        bool _cacheResults = true;
        [DataMember]
        public virtual bool CacheResults
        {
            get
            {
                return _cacheResults;
            }

            set
            {
                _cacheResults = value;
            }
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        public virtual string CacheKey
        {
            get
            {
                var key = new StringBuilder();

                key.Append("_dc:" + DocumentType);
                key.Append("_st:" + StartingRecord);
                key.Append("_en:" + RecordsToRetrieve);
                key.Append("_lc:" + Locale);
                key.Append("_cr:" + Currency);
                if (Sort != null)
                    key.Append("_st:" + Sort);

                // Add active fields
                foreach (var field in Filters)
                {
                    key.Append("_f:" + field.CacheKey);
                }

                return key.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the starting record.
        /// </summary>
        /// <value>The starting record.</value>
        [DataMember]
        public virtual int StartingRecord { get; set; }

        int _recordsToRetrieve = 50;
        /// <summary>
        /// Gets or sets the records to retrieve.
        /// </summary>
        /// <value>The records to retrieve.</value>
        [DataMember]
        public virtual int RecordsToRetrieve
        {
            get
            {
                return _recordsToRetrieve;
            }
            set
            {
                _recordsToRetrieve = value;
            }
        }

        [DataMember]
        public virtual SearchSort Sort
        {
            get;
            set;
        }

        [DataMember]
        public virtual string Locale
        {
            get;set;
        }

        [DataMember]
        public virtual string Currency
        {
            get;
            set;
        }

        public virtual string KeyField
        {
            get { return "__key"; }
        }

		public virtual string OutlineField
		{
			get { return "__outline"; }
		}

        public virtual string ReviewsTotalField
        {
            get { return "__reviewstotal"; }
        }

        public virtual string ReviewsAverageField
        {
            get { return "__reviewsavg"; }
        }

        [DataMember]
        List<ISearchFilter> _filters = new List<ISearchFilter>();

        public virtual ISearchFilter[] Filters
        {
            get { return _filters.ToArray(); }
        }

        public virtual void Add(ISearchFilter filter)
        {
            _filters.Add(filter);
        }

        [DataMember]
        List<ISearchFilter> _appliedFilters = new List<ISearchFilter>();
        public virtual ISearchFilterValue[] CurrentFilterValues
        {
            get
            {
                return null; //_CurrentFilters.Values.ToArray();
            }
        }

        public virtual ISearchFilter[] CurrentFilters
        {
            get { return _appliedFilters.ToArray(); }
        }

        public virtual string[] CurrentFilterFields
        {
            get { return (from f in _appliedFilters.ToArray() select f.Key).ToArray(); }
        }

        public virtual void Apply(ISearchFilter filter)
        {
            _appliedFilters.Add(filter);
        }

        protected SearchCriteriaBase(string documentType)
        {
            _documentType = documentType;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        protected bool IsModified
        {
            get; set;
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        protected virtual void ChangeState()
        {
            IsModified = true;
        }
    }
}

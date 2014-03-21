using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Search.Schemas;

namespace VirtoCommerce.Foundation.Search
{
    [DataContract]
    [KnownType(typeof(AttributeFilter))]
    [KnownType(typeof(RangeFilter))]
    [KnownType(typeof(PriceRangeFilter))]
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

                key.Append("_dc:" + this.DocumentType);
                key.Append("_st:" + this.StartingRecord.ToString());
                key.Append("_en:" + this.RecordsToRetrieve.ToString());
                key.Append("_lc:" + this.Locale);
                key.Append("_cr:" + this.Currency);
                if (Sort != null)
                    key.Append("_st:" + this.Sort.ToString());

                // Add active fields
                foreach (var field in this._CurrentFilters)
                {
                    key.Append("_cf:" + field.Key.Key + "|" + field.Value.Id);
                }

                return key.ToString();
            }
        }

        int _StartingRecord = 0;
        /// <summary>
        /// Gets or sets the starting record.
        /// </summary>
        /// <value>The starting record.</value>
        [DataMember]
        public virtual int StartingRecord
        {
            get
            {
                return _StartingRecord;
            }
            set
            {
                _StartingRecord = value;
            }
        }

        int _RecordsToRetrieve = 50;
        /// <summary>
        /// Gets or sets the records to retrieve.
        /// </summary>
        /// <value>The records to retrieve.</value>
        [DataMember]
        public virtual int RecordsToRetrieve
        {
            get
            {
                return _RecordsToRetrieve;
            }
            set
            {
                _RecordsToRetrieve = value;
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

        public virtual string BrowsingOutlineField
        {
            get { return "__browsingoutline"; }
        }

        [DataMember]
        List<ISearchFilter> _Filters = new List<ISearchFilter>();

        public virtual ISearchFilter[] Filters
        {
            get { return _Filters.ToArray(); }
        }

        public virtual void Add(ISearchFilter filter)
        {
            _Filters.Add(filter);
        }

        [DataMember]
        Dictionary<ISearchFilter, ISearchFilterValue> _CurrentFilters = new Dictionary<ISearchFilter, ISearchFilterValue>();

        public virtual ISearchFilterValue[] CurrentFilterValues
        {
            get { return _CurrentFilters.Values.ToArray(); }
        }

        public virtual ISearchFilter[] CurrentFilters
        {
            get { return _CurrentFilters.Keys.ToArray(); }
        }

        public virtual string[] CurrentFilterFields
        {
            get { return (from f in _CurrentFilters.Keys.ToArray() select f.Key).ToArray(); }
        }

        public virtual void Add(ISearchFilter filter, ISearchFilterValue value)
        {
            _CurrentFilters.Add(filter, value);
        }

        public SearchCriteriaBase(string documentType)
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

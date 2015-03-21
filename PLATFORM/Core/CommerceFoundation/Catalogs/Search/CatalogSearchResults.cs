using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Search.Facets;

namespace VirtoCommerce.Foundation.Catalogs.Search
{
    [DataContract, Serializable]
	[KnownType(typeof(CatalogItemSearchCriteria))]
    public class CatalogItemSearchResults
    {
        [DataMember]
        ISearchCriteria _SearchCriteria;

        public virtual ISearchCriteria SearchCriteria
        {
            get { return _SearchCriteria; }
        }

        [DataMember]
        Dictionary<string, Dictionary<string, object>> _Items;

        public virtual Dictionary<string, Dictionary<string,object>> Items
        { 
            get
            {
                return _Items;
            }
        }

        [DataMember]
        int _TotalCount = 0;

        public virtual int TotalCount
        {
            get
            {
                return _TotalCount;
            }
        }

        [DataMember]
        int _Count = 0;

        public virtual int Count
        {
            get
            {
                return _Count;
            }
        }

        [DataMember]
        FacetGroup[] _FacetGroups = null;

        public virtual FacetGroup[] FacetGroups
        {
            get
            {
                return _FacetGroups;
            }
        }

        public CatalogItemSearchResults()
        {
        }

        public CatalogItemSearchResults(ISearchCriteria criteria, Dictionary<string, Dictionary<string, object>> items, SearchResults results)
        {
            _Items = items;
            _TotalCount = results.TotalCount;
            _Count = results.DocCount;
            _FacetGroups = results.FacetGroups;
            _SearchCriteria = criteria;
        }
    }
}

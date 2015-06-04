using System.Collections.Generic;

namespace VirtoCommerce.Domain.Search.Model
{
    public class CatalogItemSearchResults
    {

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

        ISearchCriteria _SearchCriteria;

        public virtual ISearchCriteria SearchCriteria
        {
            get { return _SearchCriteria; }
        }

        Dictionary<string, Dictionary<string, object>> _Items;

        public virtual Dictionary<string, Dictionary<string,object>> Items
        { 
            get
            {
                return _Items;
            }
        }

        int _TotalCount = 0;

        public virtual int TotalCount
        {
            get
            {
                return _TotalCount;
            }
        }

        int _Count = 0;
        public virtual int Count
        {
            get
            {
                return _Count;
            }
        }

        FacetGroup[] _FacetGroups = null;
        public virtual FacetGroup[] FacetGroups
        {
            get
            {
                return _FacetGroups;
            }
        }

    
    }
}

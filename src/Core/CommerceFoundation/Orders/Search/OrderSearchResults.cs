using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Orders.Model;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Search.Facets;

namespace VirtoCommerce.Foundation.Orders.Search
{
	[DataContract, Serializable]
	[KnownType(typeof(OrderSearchCriteria))]
    public class OrderSearchResults
    {
        [DataMember]
        ISearchCriteria _SearchCriteria = null;

        public virtual ISearchCriteria SearchCriteria
        {
            get { return _SearchCriteria; }
        }

        [DataMember]
        OrderGroup[] _OrderGroups = null;

        public virtual OrderGroup[] OrderGroups
        { 
            get
            {
                return _OrderGroups;
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

        public virtual string[] OrderIds
        {
            get
            {
                if (OrderGroups == null || OrderGroups.Count() == 0)
                    return null;

                List<string> ids = new List<string>();

                foreach (OrderGroup order in OrderGroups)
                {
                    ids.Add(order.OrderGroupId);
                }

                return ids.ToArray();
            }
        }

        public OrderSearchResults()
        {
        }

        public OrderSearchResults(ISearchCriteria criteria, OrderGroup[] orders, SearchResults results)
        {
            _OrderGroups = orders;
            _TotalCount = results.TotalCount;
            _Count = results.DocCount;
            _FacetGroups = results.FacetGroups;
            _SearchCriteria = criteria;
        }
    }
}

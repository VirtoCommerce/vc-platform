using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Order
{
    public class OrderSearchResult
    {
        public List<CustomerOrder> Orders { get; set; }

        public int TotalCount { get; set; }
    }
}
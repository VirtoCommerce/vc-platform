using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class OrderSearchResult
    {
        #region Public Properties

        public List<CustomerOrder> CustomerOrders { get; set; }

        public int TotalCount { get; set; }

        #endregion
    }
}

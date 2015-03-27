using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.CustomerService
{
    public class CustomerSearchResult
    {
        #region Public Properties

        public List<Contact> Contacts { get; set; }
        public int TotalCount { get; set; }

        #endregion
    }
}

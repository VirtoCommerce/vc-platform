using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.CustomerService
{
    public class CustomerSearchResult
    {
        public int TotalCount { get; set; }

        public List<Contact> Contacts { get; set; }
    }
}
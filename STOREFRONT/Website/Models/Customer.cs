#region
using System.Collections.Generic;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class Customer : Drop
    {
        #region Constructors and Destructors
        public Customer()
        {
            this.Addresses = new List<CustomerAddress>();
        }
        #endregion

        #region Public Properties
        public bool AcceptsMarketing { get; set; }

        public List<CustomerAddress> Addresses { get; set; }

        public int AddressesCount
        {
            get
            {
                return this.Addresses == null ? 0 : this.Addresses.Count;
            }
        }

        public CustomerAddress DefaultAddress { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public bool HasAccount { get; set; }

        public string Id { get; set; }

        public string LastName { get; set; }

        public string LastOrder { get; set; }

        public string Name { get; set; }

        public List<CustomerOrder> Orders { get; set; }

        public int OrdersCount { get; set; }

        public CustomerOrder RecentOrder { get; set; }

        public string[] Tags { get; set; }

        public decimal TotalSpent { get; set; }
        #endregion
    }
}
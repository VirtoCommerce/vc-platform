#region

using System;
using System.Collections.Generic;
using System.Linq;
using DotLiquid;
using System.Threading.Tasks;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
using System.Runtime.Serialization;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Customer : Drop, ILoadSlice
    {
        private bool _ordersLoaded;
        private ItemCollection<CustomerOrder> _orders;

        #region Constructors and Destructors
        public Customer()
        {
            this.Addresses = new List<CustomerAddress>();
        }
        #endregion

        #region Public Properties
        [DataMember]
        public bool AcceptsMarketing { get; set; }

        [DataMember]
        public List<CustomerAddress> Addresses { get; set; }

        [DataMember]
        public int AddressesCount
        {
            get
            {
                return this.Addresses == null ? 0 : this.Addresses.Count;
            }
        }

        [DataMember]
        public CustomerAddress DefaultAddress { get; set; }

        [DataMember]
        public CustomerAddress NewAddress
        {
            get
            {
                return new CustomerAddress() {Id = Guid.NewGuid().ToString()};
            }
        }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public bool HasAccount { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string LastOrder { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ItemCollection<CustomerOrder> Orders
        {
            get
            {
                LoadOrders();
                return _orders;
            }
            set
            {
                _orders = value;
            }
        }

        [DataMember]
        public int OrdersCount
        {
            get
            {
                return Orders != null ? Orders.Size : 0;
            }
        }

        [DataMember]
        public CustomerOrder RecentOrder { get; set; }

        [DataMember]
        public ICollection<Tag> Tags { get; set; }

        [DataMember]
        public decimal TotalSpent { get; set; }
        #endregion

        public void LoadSlice(int from, int? to)
        {
            var pageSize = to == null ? 5 : to - from;

            var customerService = new CustomerService();

            var orderSearchResult =
                Task.Run(() => customerService.GetOrdersAsync(
                    SiteContext.Current.StoreId,
                    Id,
                    null,
                    from,
                    pageSize.Value)).Result;

            var orders = orderSearchResult.CustomerOrders.Select(o => o.AsWebModel());
            var ordersCollection = new ItemCollection<CustomerOrder>(orders)
            {
                TotalCount = orderSearchResult.TotalCount
            };

            Orders = ordersCollection;
        }

        private void LoadOrders()
        {
            if (_ordersLoaded)
            {
                return;
            }

            var pageSize = Context == null ? 5 : Context["paginate.page_size"].ToInt(5);
            var skip = Context == null ? 0 : Context["paginate.current_offset"].ToInt();

            LoadSlice(skip, pageSize + skip);

            _ordersLoaded = true;
        }
    }
}
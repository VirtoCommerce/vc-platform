using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class CustomerOrder : Operation
    {
        #region Public Properties

        public List<Address> Addresses { get; set; }
        public string ChannelId { get; set; }
        public string CustomerId { get; set; }

        public Discount Discount { get; set; }
        public string EmployeeId { get; set; }
        public string Id { get; set; }

        public List<PaymentIn> InPayments { get; set; }
        public List<LineItem> Items { get; set; }
        public string OrganizationId { get; set; }
        public List<Shipment> Shipments { get; set; }
        public string StoreId { get; set; }

        #endregion
    }
}

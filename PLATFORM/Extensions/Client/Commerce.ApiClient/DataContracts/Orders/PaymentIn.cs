using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class PaymentIn : Operation
    {
        #region Public Properties

        public List<Address> Addresses { get; set; }
        public string CustomerId { get; set; }
        public string CustomerOrderId { get; set; }
        public string GatewayCode { get; set; }
        public string Id { get; set; }

        public DateTime? IncomingDate { get; set; }
        public string OrganizationId { get; set; }

        public string OuterId { get; set; }

        public string Purpose { get; set; }

        public string ShipmentId { get; set; }

        #endregion
    }
}

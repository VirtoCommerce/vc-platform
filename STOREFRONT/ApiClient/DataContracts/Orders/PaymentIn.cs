using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class PaymentIn : Operation
    {
        public string Id { get; set; }

        public string Organization { get; set; }

        public string OrganizationId { get; set; }

        public string Customer { get; set; }

        public string CustomerId { get; set; }

        public string Purpose { get; set; }

        public string GatewayCode { get; set; }

        public DateTime? IncomingDate { get; set; }

        public string OuterId { get; set; }
    }
}
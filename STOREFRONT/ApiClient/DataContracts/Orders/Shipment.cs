using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class Shipment : Operation
    {
        public string Id { get; set; }

        public string Organization { get; set; }

        public string OrganizationId { get; set; }

        public string FulfillmentCenter { get; set; }

        public string FulfillmentCenterId { get; set; }

        public string ShipmentMethodCode { get; set; }

        public string Employee { get; set; }

        public string EmployeeId { get; set; }

        public decimal DiscountAmount { get; set; }

        public string WeightUnit { get; set; }

        public decimal? Weight { get; set; }

        public string MeasureUnit { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public string TaxType { get; set; }

        public ICollection<LineItem> Items { get; set; }

        public ICollection<PaymentIn> InPayments { get; set; }

        public Address DeliveryAddress { get; set; }

        public Discount Discount { get; set; }

        public ICollection<TaxDetail> TaxDetails { get; set; }
    }
}
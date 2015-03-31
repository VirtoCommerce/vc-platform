using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class Shipment : Operation
    {
        #region Public Properties

        public List<Address> Addresses { get; set; }
        public string CustomerOrderId { get; set; }
        public decimal? DimensionHeight { get; set; }

        public decimal? DimensionLength { get; set; }
        public string DimensionUnit { get; set; }
        public decimal? DimensionWidth { get; set; }
        public List<Discount> Discounts { get; set; }
        public string EmployeeId { get; set; }
        public string FulfilmentCenterId { get; set; }
        public string Id { get; set; }
        public List<PaymentIn> InPayments { get; set; }
        public List<LineItem> Items { get; set; }

        public string OrganizationId { get; set; }

        public string ShipmentMethodCode { get; set; }
        public decimal? VolumetricWeight { get; set; }

        public string WeightUnit { get; set; }

        public decimal? WeightValue { get; set; }

        #endregion
    }
}

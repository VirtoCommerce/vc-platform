#region

using System.Collections.Generic;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{

    #region

    #endregion

    public class Shipment
    {
        #region Public Properties

        public string Currency { get; set; }

        public Dimension Dimension { get; set; }

        public decimal DiscountTotal { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public string Id { get; set; }

        public decimal ItemSubtotal { get; set; }

        public Address RecipientAddress { get; set; }

        public string ShipmentMethodCode { get; set; }

        public decimal ShippingPrice { get; set; }

        public decimal Subtotal { get; set; }

        public bool? TaxIncluded { get; set; }

        public decimal TaxTotal { get; set; }

        public decimal Total { get; set; }

        public decimal? VolumetricWeight { get; set; }

        public string WarehouseLocation { get; set; }

        public Weight Weight { get; set; }

        #endregion
    }
}

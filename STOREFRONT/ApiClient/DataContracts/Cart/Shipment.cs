using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Cart
{
    public class Shipment
    {
        public string Id { get; set; }

        public string ShipmentMethodCode { get; set; }

        public string FulfilmentCenterId { get; set; }

        public Address DeliveryAddress { get; set; }

        public string Currency { get; set; }

        public decimal? VolumetricWeight { get; set; }

        public string WeightUnit { get; set; }

        public decimal? Weight { get; set; }

        public string MeasureUnit { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public bool TaxIncluded { get; set; }

        public decimal ShippingPrice { get; set; }

        public decimal Total { get; set; }

        public decimal DiscountTotal { get; set; }

        public decimal TaxTotal { get; set; }

        public decimal ItemSubtotal { get; set; }

        public decimal Subtotal { get; set; }

        public ICollection<Discount> Discounts { get; set; }

        public ICollection<CartItem> Items { get; set; }

        public string TaxType { get; set; }

        public ICollection<TaxDetail> TaxDetails { get; set; }
    }
}
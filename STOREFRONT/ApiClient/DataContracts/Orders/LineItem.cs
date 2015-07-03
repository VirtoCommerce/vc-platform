using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Orders
{
    public class LineItem
    {
        public string Id { get; set; }

        public decimal BasePrice { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal Tax { get; set; }

        public string Currency { get; set; }

        public int ReserveQuantity { get; set; }

        public int Quantity { get; set; }

        public string ProductId { get; set; }

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string DisplayName { get; set; }

        public bool IsGift { get; set; }

        public string ShippingMethodCode { get; set; }

        public string FulfilmentLocationCode { get; set; }

        public string WeightUnit { get; set; }

        public decimal? Weight { get; set; }

        public string MeasureUnit { get; set; }

        public decimal? Height { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public string TaxType { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledDate { get; set; }

        public string CancelReason { get; set; }

        public Discount Discount { get; set; }

        public ICollection<TaxDetail> TaxDetails { get; set; }
    }
}
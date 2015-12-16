using System;
using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Order
{
    /// <summary>
    /// Represents order line item
    /// </summary>
    public class LineItem
    {
        public LineItem()
        {
            TaxDetails = new List<TaxDetail>();
            DynamicProperties = new List<DynamicProperty>();
        }

        /// <summary>
        /// Price where tax and discount are not applied
        /// </summary>
        /// <value>Price with tax and without dicount</value>
        public Money BasePrice { get; set; }

        /// <summary>
        /// Price where tax is not applied and discount is applied
        /// </summary>
        /// <value>Price with tax and discount</value>
        public Money Price { get; set; }

        /// <summary>
        /// Discount amount
        /// </summary>
        /// <value>Discount amount</value>
        public Money DiscountAmount { get; set; }

        /// <summary>
        /// Tax sum
        /// </summary>
        /// <value>Tax sum</value>
        public Money Tax { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Reserve quantity
        /// </summary>
        /// <value>Reserve quantity</value>
        public int? ReserveQuantity { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets ImageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or Sets DisplayName
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or Sets IsGift
        /// </summary>
        public bool? IsGift { get; set; }

        /// <summary>
        /// Gets or Sets ShippingMethodCode
        /// </summary>
        public string ShippingMethodCode { get; set; }

        /// <summary>
        /// Gets or Sets FulfilmentLocationCode
        /// </summary>
        public string FulfilmentLocationCode { get; set; }

        /// <summary>
        /// Gets or Sets WeightUnit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Gets or Sets MeasureUnit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// Gets or Sets Length
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// Gets or Sets Width
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// Gets or Sets TaxType
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Flag represent that line item was canceled
        /// </summary>
        /// <value>Flag represent that line item was canceled</value>
        public bool? IsCancelled { get; set; }

        /// <summary>
        /// Gets or Sets CancelledDate
        /// </summary>
        public DateTime? CancelledDate { get; set; }

        /// <summary>
        /// Text representation of cancel reason
        /// </summary>
        /// <value>Text representation of cancel reason</value>
        public string CancelReason { get; set; }

        /// <summary>
        /// Gets or Sets Discount
        /// </summary>
        public Discount Discount { get; set; }

        /// <summary>
        /// Gets or Sets TaxDetails
        /// </summary>
        public ICollection<TaxDetail> TaxDetails { get; set; }

        /// <summary>
        /// Used for dynamic properties management, contains object type string
        /// </summary>
        /// <value>Used for dynamic properties management, contains object type string</value>
        public string ObjectType { get; set; }

        /// <summary>
        /// Dynamic properties collections
        /// </summary>
        /// <value>Dynamic properties collections</value>
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string Id { get; set; }
    }
}
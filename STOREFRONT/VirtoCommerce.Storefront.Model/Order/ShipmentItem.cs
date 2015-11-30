using System;

namespace VirtoCommerce.Storefront.Model.Order
{
    /// <summary>
    /// Represents order shipment item
    /// </summary>
    public class ShipmentItem
    {
        /// <summary>
        /// Gets or Sets LineItemId
        /// </summary>
        public string LineItemId { get; set; }

        /// <summary>
        /// Gets or Sets LineItem
        /// </summary>
        public LineItem LineItem { get; set; }

        /// <summary>
        /// Gets or Sets BarCode
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        public int? Quantity { get; set; }

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
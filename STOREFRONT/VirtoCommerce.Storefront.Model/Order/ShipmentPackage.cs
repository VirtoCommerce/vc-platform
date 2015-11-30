using System;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model.Order
{
    /// <summary>
    /// Represents order shipment package
    /// </summary>
    public class ShipmentPackage
    {
        public ShipmentPackage()
        {
            Items = new List<ShipmentItem>();
        }

        /// <summary>
        /// Gets or Sets BarCode
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// Gets or Sets PackageType
        /// </summary>
        public string PackageType { get; set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        public ICollection<ShipmentItem> Items { get; set; }

        /// <summary>
        /// Gets or Sets WeightUnit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        public double? Weight { get; set; }

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
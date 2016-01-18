using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class Inventory : ValueObject<Inventory>
    {
        /// <summary>
        /// Gets or Sets PreorderAvailabilityDate
        /// </summary>
        public DateTime? PreorderAvailabilityDate { get; set; }

        /// <summary>
        /// Gets or Sets BackorderAvailabilityDate
        /// </summary>
        public DateTime? BackorderAvailabilityDate { get; set; }

        /// <summary>
        /// Gets or Sets AllowPreorder
        /// </summary>
        public bool? AllowPreorder { get; set; }

        /// <summary>
        /// Gets or Sets AllowBackorder
        /// </summary>
        public bool? AllowBackorder { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        public InventoryStatus Status { get; set; }

        /// <summary>
        /// Gets or Sets FulfillmentCenterId
        /// </summary>
        public string FulfillmentCenterId { get; set; }

        /// <summary>
        /// Gets or Sets InStockQuantity
        /// </summary>
        public long? InStockQuantity { get; set; }

        /// <summary>
        /// Gets or Sets ReservedQuantity
        /// </summary>
        public long? ReservedQuantity { get; set; }

        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        public string ProductId { get; set; }
    }
}

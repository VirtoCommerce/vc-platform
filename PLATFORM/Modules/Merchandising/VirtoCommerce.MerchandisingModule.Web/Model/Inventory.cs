using System;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Inventory
    {
        /// <summary>
        /// Gets or sets the value of fulfillment canter id
        /// </summary>
        public string FulfillmentCenterId { get; set; }

        /// <summary>
        /// Gets or sets the value of inventory quantity in stock
        /// </summary>
        public long InStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of reserved inventory quantity
        /// </summary>
        public long ReservedQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of reorder inventory minimum quanitity
        /// </summary>
        public long ReorderMinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of preorder inventory quantity
        /// </summary>
        public long PreorderQuantity { get; set; }

        /// <summary>
        /// Gets or sets the value of backorder inventory quantity
        /// </summary>
        public long BackorderQuantity { get; set; }

        /// <summary>
        /// Gets or sets the flag of backorder is allowed
        /// </summary>
        public bool AllowBackorder { get; set; }

        /// <summary>
        /// Gets or sets the flag of preorder is allowed
        /// </summary>
        public bool AllowPreorder { get; set; }

        /// <summary>
        /// Gets or sets the value for inventory quantity in transit
        /// </summary>
        public long InTransit { get; set; }

        /// <summary>
        /// Gets or sets the value of date/time limit for preorder availability
        /// </summary>
        public DateTime? PreorderAvailabilityDate { get; set; }

        /// <summary>
        /// Gets or sets the value of date/time limit for backorder availability
        /// </summary>
        public DateTime? BackorderAvailabilityDate { get; set; }

        /// <summary>
        /// Gets or sets the value of inventory status
        /// </summary>
        public int Status { get; set; }
    }
}
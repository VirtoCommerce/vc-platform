using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Discount : ValueObject<Discount>
    {
        /// <summary>
        /// Gets or sets the value of promotion id
        /// </summary>
        public string PromotionId { get; set; }

        /// <summary>
        /// Gets or sets the value of absolute discount amount
        /// </summary>
        public Money Amount { get; set; }

        /// <summary>
        /// Gets or sets the value of discount description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets ID of line item for which discount was applied
        /// </summary>
        public string LineItemId { get; set; }

        /// <summary>
        /// Gets or sets ID of shopping cart for which discount was applied
        /// </summary>
        public string ShoppingCartId { get; set; }

        /// <summary>
        /// Gets or sets ID of shipment for which discount was applied
        /// </summary>
        public string ShipmentId { get; set; }
    }
}
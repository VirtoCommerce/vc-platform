﻿using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Marketing
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
        /// Gets or sets the discount type
        /// </summary>
        public PromotionRewardType Type { get; set; }
    }
}
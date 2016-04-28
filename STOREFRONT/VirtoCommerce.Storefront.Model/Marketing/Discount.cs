using System;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Marketing
{
    public class Discount : ValueObject<Discount>, IConvertible<Discount>
    {
        /// <summary>
        /// Gets or sets the value of promotion id
        /// </summary>
        public string PromotionId { get; set; }

        /// <summary>
        /// Gets or sets the value of absolute discount amount per one item
        /// </summary>
        public Money Amount { get; set; }

        /// <summary>
        /// Gets or sets the value of discount description
        /// </summary>
        public string Description { get; set; }

        #region IConvertible<Discount> Members
        public Discount ConvertTo(Currency currency)
        {
            var retVal = new Discount();
            retVal.PromotionId = PromotionId;
            retVal.Description = Description;
            retVal.Amount = Amount.ConvertTo(currency);
            return retVal;
        } 
        #endregion
    }
}
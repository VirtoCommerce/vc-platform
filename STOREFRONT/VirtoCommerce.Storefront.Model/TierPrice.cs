using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model
{
    public class TierPrice : ValueObject<TierPrice>
    {
        public TierPrice(Currency currency)
            :this (new Money(currency), 0)
        {
        }
        public TierPrice(Money price, long quantity)
        {
            Price = price;
            Quantity = quantity;
        }

        public Money Price { get; set; }
        /// <summary>
        /// Current active discount
        /// </summary>
        public Discount ActiveDiscount { get; set; }

        /// <summary>
        /// Actual price includes all kind of discounts
        /// </summary>
        public Money ActualPrice
        {
            get
            {
                var retVal = Price;
                if(ActiveDiscount != null)
                {
                    retVal -= ActiveDiscount.Amount;
                }
                return retVal;
            }
        }

        public long Quantity { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model
{
    public class ShippingMethod : ValueObject<ShippingMethod>, ITaxable, IDiscountable
    {
        public ShippingMethod()
        {
            Discounts = new List<Discount>();
        }
        public ShippingMethod(Currency currency)
            :this()
        {
            Currency = currency;
            Price = new Money(currency);           
            TaxTotal = new Money(currency);
        }
        /// <summary>
        /// Gets or sets the value of shipping method code
        /// </summary>
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option name
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option description
        /// </summary>
        public string OptionDescription { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method logo absolute URL
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// price without discount and taxes
        /// </summary>
        public Money Price { get; set; }
        /// <summary>
        ///  price with tax but without discount
        /// </summary>
        public Money PriceWithTax
        {
            get
            {
                return Price + TaxTotal;
            }
        }
        /// <summary>
        /// Resulting price with discount but without tax
        /// </summary>
        public  Money Total
        {
            get
            {
                return Price - DiscountTotal;
            }
        }
        /// <summary>
        /// Resulting price with discount and tax
        /// </summary>
        public Money TotalWithTax
        {
            get
            {
                return PriceWithTax - DiscountTotalWithTax;
            }
        }

        /// <summary>
        /// Total discount amount without tax
        /// </summary>
        public Money DiscountTotal
        {
            get
            {
                var discountTotal = Discounts.Sum(d => d.Amount.Amount);

                return new Money(discountTotal, Currency);
            }
        }
        /// <summary>
        /// Total discount amount with tax
        /// </summary>
        public Money DiscountTotalWithTax
        {
            get
            {
                var discountTotalWithTax = Discounts.Sum(d => d.AmountWithTax.Amount);

                return new Money(discountTotalWithTax, Currency);
            }
        }


        #region ITaxable Members
        /// <summary>
        /// Gets or sets the value of total shipping tax amount
        /// </summary>
        public Money TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping tax type
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax details lines
        /// </summary>
        /// <value>
        /// Collection of TaxDetail objects
        /// </value>
        public ICollection<TaxDetail> TaxDetails { get; set; }

        public void ApplyTaxRates(IEnumerable<TaxRate> taxRates)
        {
            var shippingMethodTaxRates = taxRates.Where(x => x.Line.Id == ShipmentMethodCode);
            TaxTotal = new Money(Currency);
            if (shippingMethodTaxRates.Any())
            {
                var shippingMethodTaxRate = shippingMethodTaxRates.First();
              
                TaxTotal += shippingMethodTaxRate.Rate;
            }
        }
        #endregion

        #region IDiscountable Members
        public ICollection<Discount> Discounts { get; private set; }

        public Currency Currency { get; set; }

        public void ApplyRewards(IEnumerable<PromotionReward> rewards)
        {
            var shipmentRewards = rewards.Where(r => r.RewardType == PromotionRewardType.ShipmentReward && (r.ShippingMethodCode.IsNullOrEmpty() || r.ShippingMethodCode.EqualsInvariant(ShipmentMethodCode)));

            Discounts.Clear();

            foreach (var reward in shipmentRewards)
            {
                var discount = reward.ToDiscountModel(Price, PriceWithTax);

                if (reward.IsValid)
                {
                    Discounts.Add(discount);
                }
            }
        }
        #endregion
    }
}
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.PromotionEvaluator;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Cart
{
    public class ShoppingCart : Entity, IDiscountable
    {
        public ShoppingCart(Currency currency, Language language)
        {
            Currency = currency;
            LanguageCode = language.CultureName;
            DiscountTotal = new Money(currency.Code);
            HandlingTotal = new Money(currency.Code);
            ShippingTotal = new Money(currency.Code);
            SubTotal = new Money(currency.Code);
            TaxTotal = new Money(currency.Code);
            Total = new Money(currency.Code);

            Addresses = new List<Address>();
            Discounts = new List<Discount>();
            Items = new List<LineItem>();
            Payments = new List<Payment>();
            Shipments = new List<Shipment>();
            TaxDetails = new List<TaxDetail>();
            DynamicProperties = new List<DynamicProperty>();
        }

        /// <summary>
        /// Gets or sets the value of shopping cart name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of store id
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the value of channel id
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the sign that shopping cart contains line items which require shipping
        /// </summary>
        public bool HasPhysicalProducts { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is anonymous
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart customer name
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart organization id
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart coupon
        /// </summary>
        /// <value>
        /// Coupon object
        /// </value>
        public Coupon Coupon { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart language code
        /// </summary>
        /// <value>
        /// Culture name in ISO 3166-1 alpha-3 format
        /// </value>
        public string LanguageCode { get; private set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart has tax
        /// </summary>
        public bool TaxIncluded { get; set; }

        /// <summary>
        /// Gets or sets the flag of shopping cart is recurring
        /// </summary>
        public bool IsRecuring { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart text comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the value of volumetric weight
        /// </summary>
        public decimal VolumetricWeight { get; set; }

        /// <summary>
        /// Gets or sets the value of weight unit
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart weight
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the value of measurement unit
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or sets the value of height
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// Gets or sets the value of length
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Gets or sets the value of width
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart total cost
        /// </summary>
        public Money Total { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart subtotal
        /// </summary>
        public Money SubTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping total cost
        /// </summary>
        public Money ShippingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of handling total cost
        /// </summary>
        public Money HandlingTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total discount amount
        /// </summary>
        public Money DiscountTotal { get; set; }

        /// <summary>
        /// Gets or sets the value of total tax cost
        /// </summary>
        public Money TaxTotal { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart addresses
        /// </summary>
        /// <value>
        /// Collection of Address objects
        /// </value>
        public ICollection<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the default shipping address
        /// </summary>
        public Address DefaultShippingAddress { get; set; }

        /// <summary>
        /// Gets default the default billing address
        /// </summary>
        public Address DefaultBillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the value of shopping cart line items
        /// </summary>
        /// <value>
        /// Collection of LineItem objects
        /// </value>
        public ICollection<LineItem> Items { get; set; }

        /// <summary>
        /// Gets or sets shopping cart items quantity (sum of each line item quantity * items count)
        /// </summary>
        public int ItemsCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart payments
        /// </summary>
        /// <value>
        /// Collection of Payment objects
        /// </value>
        public ICollection<Payment> Payments { get; set; }

        /// <summary>
        /// Gets or sets the collection of shopping cart shipments
        /// </summary>
        /// <value>
        /// Collection of Shipment objects
        /// </value>
        public ICollection<Shipment> Shipments { get; set; }

        /// <summary>
        /// Gets or sets the collection of line item tax detalization lines
        /// </summary>
        /// <value>
        /// Collection of TaxDetail objects
        /// </value>
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

        public ICollection<Discount> Discounts { get; }

        public Currency Currency { get; }

        public void ApplyRewards(IEnumerable<PromotionReward> rewards)
        {
            Discounts.Clear();

            var cartRewards = rewards.Where(r => r.RewardType == PromotionRewardType.CartSubtotalReward);
            foreach (var reward in cartRewards)
            {
                var discount = reward.ToDiscountModel(SubTotal.Amount, Currency);

                if (reward.IsValid)
                {
                    Discounts.Add(discount);
                }
            }

            var lineItemRewards = rewards.Where(r => r.RewardType == PromotionRewardType.CatalogItemAmountReward);
            foreach (var lineItem in Items)
            {
                lineItem.ApplyRewards(lineItemRewards);
            }

            var shipmentRewards = rewards.Where(r => r.RewardType == PromotionRewardType.ShipmentReward);
            foreach (var shipment in Shipments)
            {
                shipment.ApplyRewards(shipmentRewards);
            }

            if (Coupon != null && !string.IsNullOrEmpty(Coupon.Code))
            {
                var couponReward = rewards.FirstOrDefault(r => r.Promotion.Coupons != null && r.Promotion.Coupons.Any());
                if (couponReward != null)
                {
                    var discount = couponReward.ToDiscountModel(SubTotal.Amount, Currency);
                    string couponCode = couponReward.Promotion.Coupons.FirstOrDefault(c => c == Coupon.Code);
                    if (!string.IsNullOrEmpty(couponCode))
                    {
                        Coupon.Amount = discount.Amount;
                        Coupon.AppliedSuccessfully = couponReward.IsValid;
                        Coupon.Code = couponCode;
                        Coupon.Description = couponReward.Promotion.Description;
                    }
                }
                else
                {
                    Coupon.ErrorCode = "InvalidCouponCode";
                }
            }
        }
    }
}
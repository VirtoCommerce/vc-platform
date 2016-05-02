using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Cart.ValidationErrors;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Cart
{
    public class ShoppingCart : Entity, IDiscountable, IValidatable, IHasLanguage
    {
        public ShoppingCart(Currency currency, Language language)
        {
            Currency = currency;
            Language = language;
            HandlingTotal = new Money(currency);
            TaxTotal = new Money(currency);

            Addresses = new List<Address>();
            Discounts = new List<Discount>();
            Items = new List<LineItem>();
            Payments = new List<Payment>();
            Shipments = new List<Shipment>();
            TaxDetails = new List<TaxDetail>();
            DynamicProperties = new List<DynamicProperty>();
            ValidationErrors = new List<ValidationError>();
            ValidationWarnings = new List<ValidationError>();
            AvailablePaymentMethods = new List<PaymentMethod>();
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

        public CustomerInfo Customer { get; set; }

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
        /// Gets the value of shopping cart total cost
        /// </summary>
        public Money Total
        {
            get
            {
                return SubTotal + TaxTotal + ShippingTotal - DiscountTotal;
            }
        }

        /// <summary>
        /// Gets the value of shopping cart subtotal
        /// </summary>
        public Money SubTotal
        {
            get
            {
                var subtotal = Items.Sum(i => i.ListPrice.Amount * i.Quantity);

                return new Money(subtotal, Currency);
            }
        }

        /// <summary>
        /// Gets the value of shopping cart items total extended price (product price includes all kinds of discounts)
        /// </summary>
        public Money ExtendedPriceTotal
        {
            get
            {
                var extendedPriceTotal = Items.Sum(i => i.ExtendedPrice.Amount);

                return new Money(extendedPriceTotal, Currency);
            }
        }

        /// <summary>
        /// Gets the value of shipping total cost
        /// </summary>
        public Money ShippingTotal
        {
            get
            {
                var shippingTotal = Shipments.Sum(s => s.Total.Amount);

                return new Money(shippingTotal, Currency);
            }
        }

        /// <summary>
        /// Gets or sets the value of handling total cost
        /// </summary>
        public Money HandlingTotal { get; set; }

        /// <summary>
        /// Gets the value of total discount amount
        /// </summary>
        public Money DiscountTotal
        {
            get
            {
                var discountTotal = Discounts.Sum(d => d.Amount.Amount);
                var itemDiscountTotal = Items.Sum(i => i.DiscountTotal.Amount);
                var shipmentDiscountTotal = Shipments.Sum(s => s.DiscountTotal.Amount);
  
                return new Money(discountTotal + itemDiscountTotal + shipmentDiscountTotal, Currency);
            }
        }

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
        public Address DefaultShippingAddress
        {
            get
            {
                Address shippingAddress = null;

                if (HasPhysicalProducts)
                {
                    var shipment = Shipments.FirstOrDefault();
                    if (shipment != null)
                    {
                        shippingAddress = shipment.DeliveryAddress;
                    }

                    if (shippingAddress == null && Customer != null)
                    {
                        shippingAddress = Customer.Addresses.FirstOrDefault();
                    }

                    if (shippingAddress == null)
                    {
                        shippingAddress = new Address
                        {
                            Type = AddressType.Shipping,
                            Email = Customer.Email,
                            FirstName = Customer.FirstName,
                            LastName = Customer.LastName
                        };
                    }

                    shippingAddress.Type = AddressType.Shipping;
                }

                return shippingAddress;
            }
        }

        /// <summary>
        /// Gets default the default billing address
        /// </summary>
        public Address DefaultBillingAddress
        {
            get
            {
                Address billingAddress = null;

                var payment = Payments.FirstOrDefault();
                if (payment != null)
                {
                    billingAddress = payment.BillingAddress;
                }

                if (billingAddress == null && Customer != null)
                {
                    billingAddress = Customer.Addresses.FirstOrDefault();
                }

                if (billingAddress == null)
                {
                    billingAddress = new Address
                    {
                        Type = AddressType.Billing,
                        Email = Customer.Email,
                        FirstName = Customer.FirstName,
                        LastName = Customer.LastName
                    };
                }

                billingAddress.Type = AddressType.Billing;

                return billingAddress;
            }
        }

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
        public int ItemsCount
        {
            get
            {
                return Items.Sum(i => i.Quantity);
            }
        }
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
        /// Used for dynamic properties management, contains object type string
        /// </summary>
        /// <value>Used for dynamic properties management, contains object type string</value>

        public string ObjectType { get; set; }

        /// <summary>
        /// Dynamic properties collections
        /// </summary>
        /// <value>Dynamic properties collections</value>
        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        /// <summary>
        /// Gets or sets the cart validation type
        /// </summary>
        public ValidationType ValidationType { get; set; }

        public bool HasValidationErrors
        {
            get
            {
                return ValidationErrors.Any() || Items.Where(i => i.ValidationErrors.Any()).Any() || Shipments.Where(s => s.ValidationErrors.Any()).Any();
            }
        }

        public ICollection<PaymentMethod> AvailablePaymentMethods { get; set; }

        public LineItem RecentlyAddedItem
        {
            get
            {
                return Items.OrderByDescending(i => i.CreatedDate).FirstOrDefault();
            }
        }

        public string Email
        {
            get
            {
                string email = null;

                var shipment = Shipments.FirstOrDefault();
                if (shipment != null && shipment.DeliveryAddress != null)
                {
                    email = shipment.DeliveryAddress.Email;
                }

                if (string.IsNullOrEmpty(email))
                {
                    var payment = Payments.FirstOrDefault();
                    if (payment != null && payment.BillingAddress != null)
                    {
                        email = payment.BillingAddress.Email;
                    }
                }

                if (string.IsNullOrEmpty(email))
                {
                    email = Customer.Email;
                }

                return email;
            }
        }

        public ICollection<ValidationError> ValidationErrors { get; set; }

        public ICollection<ValidationError> ValidationWarnings { get; set; }

        #region IDiscountable Members
        public ICollection<Discount> Discounts { get; private set; }

        public Currency Currency { get; private set; }

        public void ApplyRewards(IEnumerable<PromotionReward> rewards)
        {
            Discounts.Clear();

            var cartRewards = rewards.Where(r => r.RewardType == PromotionRewardType.CartSubtotalReward);
            foreach (var reward in cartRewards)
            {
                var discount = reward.ToDiscountModel(SubTotal);

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
                var couponRewards = rewards.Where(r => r.Promotion.Coupons != null && r.Promotion.Coupons.Any());
                if (!couponRewards.Any())
                {
                    Coupon.AppliedSuccessfully = false;
                    Coupon.ErrorCode = "InvalidCouponCode";
                }
                foreach (var reward in couponRewards)
                {
                    var couponCode = reward.Promotion.Coupons.FirstOrDefault(c => c == Coupon.Code);
                    if (!string.IsNullOrEmpty(couponCode))
                    {
                        Coupon.AppliedSuccessfully = reward.IsValid;
                        Coupon.Description = reward.Promotion.Description;
                    }
                }
            }
        }
        #endregion


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
            TaxTotal = new Money(TaxTotal.Currency);
            foreach (var taxRate in taxRates)
            {
                TaxTotal += taxRate.Rate;
            }
            foreach(var lineItem in Items)
            {
                lineItem.ApplyTaxRates(taxRates);
            }
            foreach(var shipment in Shipments)
            {
                shipment.ApplyTaxRates(taxRates);
            }
        }
        #endregion

        #region IHasLanguage Members
        public Language Language { get; set; }
        #endregion

        public override string ToString()
        {
            return string.Format("Cart #{0} Items({1}) {2}", Id ?? "undef", ItemsCount, Customer != null ? Customer.ToString() : "undef"); 
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Web.Client.Extensions.Validation;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Virto.Helpers;
using VirtoCommerce.Web.Virto.Helpers.Popup;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CheckoutModel.
	/// </summary>
    public class CheckoutModel : PopupBase
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CheckoutModel"/> class.
		/// </summary>
        public CheckoutModel()
        {
            AcceptTerms = false;
            UseForShipping = true;
            CreateAccount = false;
            IncludeGiftMessage = false;
            CouponCode = UserHelper.CustomerSession.CouponCode;
        }

		/// <summary>
		/// Gets or sets the billing address.
		/// </summary>
		/// <value>The billing address.</value>
        public CheckoutAddressModel BillingAddress { get; set; }

		/// <summary>
		/// Gets or sets the shipping address.
		/// </summary>
		/// <value>The shipping address.</value>
        public CheckoutAddressModel ShippingAddress { get; set; }

		/// <summary>
		/// Gets the gift message.
		/// </summary>
		/// <value>The gift message.</value>
        public GiftMessageModel GiftMessage
        {
            get { return new GiftMessageModel(); }
        }

		/// <summary>
		/// Gets or sets a value indicating whether [use for shipping].
		/// </summary>
		/// <value><c>true</c> if [use for shipping]; otherwise, <c>false</c>.</value>
        public bool UseForShipping { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [create account].
		/// </summary>
		/// <value><c>true</c> if [create account]; otherwise, <c>false</c>.</value>
        public bool CreateAccount { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [include gift message].
		/// </summary>
		/// <value><c>true</c> if [include gift message]; otherwise, <c>false</c>.</value>
        public bool IncludeGiftMessage { get; set; }
		/// <summary>
		/// Gets or sets the feedback message.
		/// </summary>
		/// <value>The feedback message.</value>
        public string FeedbackMessage { get; set; }
		/// <summary>
		/// Gets or sets the coupon code.
		/// </summary>
		/// <value>The coupon code.</value>
        public string CouponCode { get; set; }
		/// <summary>
		/// Gets or sets the comments.
		/// </summary>
		/// <value>The comments.</value>
        public string Comments { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [accept terms].
		/// </summary>
		/// <value><c>true</c> if [accept terms]; otherwise, <c>false</c>.</value>
        [BooleanRequired(ErrorMessage = "Please accept the terms")]
        public bool AcceptTerms { get; set; }

		/// <summary>
		/// Gets or sets the shipping method.
		/// </summary>
		/// <value>The shipping method.</value>
        [Required]
        public string ShippingMethod { get; set; }

		/// <summary>
		/// Gets or sets the payment method.
		/// </summary>
		/// <value>The payment method.</value>
        [Required]
        public string PaymentMethod { get; set; }

		/// <summary>
		/// Gets or sets the payments.
		/// </summary>
		/// <value>The payments.</value>
        public PaymentModel[] Payments { get; set; }

		/// <summary>
		/// Gets or sets the address book.
		/// </summary>
		/// <value>The address book.</value>
        public AddressBook AddressBook { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

	/// <summary>
	/// Class AddressBook.
	/// </summary>
    public class AddressBook
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="AddressBook"/> class.
		/// </summary>
        public AddressBook()
        {
            SaveBillingAddress = false;
            SaveShippingAddress = false;
        }

		/// <summary>
		/// Gets or sets the billing address identifier.
		/// </summary>
		/// <value>The billing address identifier.</value>
        public string BillingAddressId { get; set; }
		/// <summary>
		/// Gets or sets the shipping address identifier.
		/// </summary>
		/// <value>The shipping address identifier.</value>
        public string ShippingAddressId { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [save billing address].
		/// </summary>
		/// <value><c>true</c> if [save billing address]; otherwise, <c>false</c>.</value>
        public bool SaveBillingAddress { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether [save shipping address].
		/// </summary>
		/// <value><c>true</c> if [save shipping address]; otherwise, <c>false</c>.</value>
        public bool SaveShippingAddress { get; set; }
		/// <summary>
		/// Gets or sets the addresses.
		/// </summary>
		/// <value>The addresses.</value>
        public Address[] Addresses { get; set; }
    }

	/// <summary>
	/// Class PaymentsModel.
	/// </summary>
    public class PaymentsModel
    {
		/// <summary>
		/// Gets or sets the payment method.
		/// </summary>
		/// <value>The payment method.</value>
		[Required(ErrorMessage = "Pick a payment method")]
        public string PaymentMethod { get; set; }

		/// <summary>
		/// Gets or sets the payments.
		/// </summary>
		/// <value>The payments.</value>
        public PaymentModel[] Payments { get; set; }
    }

	/// <summary>
	/// Class ShipmentsModel.
	/// </summary>
    public class ShipmentsModel
    {
		/// <summary>
		/// Gets or sets the shipping method.
		/// </summary>
		/// <value>The shipping method.</value>
		[Required(ErrorMessage = "Pick a shipping method")]
        public string ShippingMethod { get; set; }

		/// <summary>
		/// Gets or sets the shipments.
		/// </summary>
		/// <value>The shipments.</value>
        public ShippingMethodModel[] Shipments { get; set; }
    }

	/// <summary>
	/// Class PaymentModel.
	/// </summary>
    public class PaymentModel
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
	    public string Id { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }
		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// <value>The display name.</value>
        public string DisplayName { get; set; }

		/// <summary>
		/// Gets or sets the name of the customer.
		/// </summary>
		/// <value>The name of the customer.</value>
        [Required]
		[Display(Name = "Name on Card")]
        public string CustomerName { get; set; }

		/// <summary>
		/// Gets or sets the type of the card.
		/// </summary>
		/// <value>The type of the card.</value>
        [Required]
		[Display(Name = "Credit Card Type")]
        public string CardType { get; set; }

		/// <summary>
		/// Gets or sets the card number.
		/// </summary>
		/// <value>The card number.</value>
        [DataAnnotationsExtensions.CreditCard, Required]
		[Display(Name = "Credit Card Number")]
        public string CardNumber { get; set; }

		/// <summary>
		/// Gets or sets the expiration month.
		/// </summary>
		/// <value>The expiration month.</value>
        [Range(1, 12), Required]
		[Display(Name = "Expiration Month")]
        public int ExpirationMonth { get; set; }

		/// <summary>
		/// Gets or sets the expiration year.
		/// </summary>
		/// <value>The expiration year.</value>
        [Required]
        public int ExpirationYear { get; set; }

		/// <summary>
		/// Gets or sets the card verification number.
		/// </summary>
		/// <value>The card verification number.</value>
        [Required]
		[Display(Name = "Card Verification Number")]
        public string CardVerificationNumber { get; set; }


		/// <summary>
		/// Gets or sets the months.
		/// </summary>
		/// <value>The months.</value>
        public ListModel[] Months { get; set; }
		/// <summary>
		/// Gets or sets the years.
		/// </summary>
		/// <value>The years.</value>
        public ListModel[] Years { get; set; }
		/// <summary>
		/// Gets or sets the card types.
		/// </summary>
		/// <value>The card types.</value>
        public ListModel[] CardTypes { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is current.
		/// </summary>
		/// <value><c>true</c> if this instance is current; otherwise, <c>false</c>.</value>
	    public bool IsCurrent { get; set; }	
    }

	/// <summary>
	/// Class ListModel.
	/// </summary>
    public class ListModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ListModel"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="val">The value.</param>
        public ListModel(string name, string val)
        {
            Name = name;
            Value = val;
        }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }
		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
        public string Value { get; set; }
    }

	/// <summary>
	/// Class CheckoutAddressModel.
	/// </summary>
    public class CheckoutAddressModel
    {
		/// <summary>
		/// The _regions
		/// </summary>
        private CountryRegionScriptModel[] _regions;
		/// <summary>
		/// Gets or sets the countries.
		/// </summary>
		/// <value>The countries.</value>
        public Country[] Countries { get; set; }
		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
        public AddressModel Address { get; set; }

		/// <summary>
		/// Gets the script country regions.
		/// </summary>
		/// <value>The script country regions.</value>
        public CountryRegionScriptModel[] ScriptCountryRegions
        {
            get
            {
                if (_regions != null)
                {
                    return _regions;
                }

                var regions = from c in Countries
                              select
                                  new CountryRegionScriptModel(c.CountryId,
                                                               (from r in c.Regions
                                                                select new RegionScriptModel(r.RegionId, r.DisplayName))
                                                                   .ToArray());

                _regions = regions.ToArray();

                return _regions;
            }
        }
    }

	/// <summary>
	/// Class CountryRegionScriptModel.
	/// </summary>
    public class CountryRegionScriptModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CountryRegionScriptModel"/> class.
		/// </summary>
		/// <param name="countryId">The country identifier.</param>
		/// <param name="regions">The regions.</param>
        public CountryRegionScriptModel(string countryId, RegionScriptModel[] regions)
        {
            CountryId = countryId;
            Regions = regions;
        }

		/// <summary>
		/// Gets or sets the country identifier.
		/// </summary>
		/// <value>The country identifier.</value>
        public string CountryId { get; set; }
		/// <summary>
		/// Gets or sets the regions.
		/// </summary>
		/// <value>The regions.</value>
        public RegionScriptModel[] Regions { get; set; }
    }

	/// <summary>
	/// Class RegionScriptModel.
	/// </summary>
    public class RegionScriptModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="RegionScriptModel"/> class.
		/// </summary>
		/// <param name="regionId">The region identifier.</param>
		/// <param name="name">The name.</param>
        public RegionScriptModel(string regionId, string name)
        {
            RegionId = regionId;
            Name = name;
        }

		/// <summary>
		/// Gets or sets the region identifier.
		/// </summary>
		/// <value>The region identifier.</value>
        public string RegionId { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }
    }

	/// <summary>
	/// Class GiftMessageModel.
	/// </summary>
    public class GiftMessageModel
    {
		/// <summary>
		/// Gets or sets from.
		/// </summary>
		/// <value>From.</value>
        [Required, StringLength(100)]
        public string From { get; set; }

		/// <summary>
		/// Gets or sets to.
		/// </summary>
		/// <value>To.</value>
        [Required, StringLength(100)]
        public string To { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
        [Required, StringLength(500)]
        public string Message { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
        public string Type { get; set; }
    }

	/// <summary>
	/// Class ShippingMethodModel.
	/// </summary>
    public class ShippingMethodModel : IComparable<ShippingMethodModel>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ShippingMethodModel"/> class.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="cart">The cart.</param>
        public ShippingMethodModel(ShippingMethod method, ShoppingCart cart)
        {
            Method = method;
            Cart = cart;
        }

		/// <summary>
		/// Gets or sets the method.
		/// </summary>
		/// <value>The method.</value>
        [JsonIgnore]
        public ShippingMethod Method { get; set; }

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id
        {
            get { return Method.ShippingMethodId; }
        }

		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <value>The display name.</value>
       public string DisplayName
        {
            get
            {
                var shippingMethodLanguage =
                    Method.ShippingMethodLanguages.FirstOrDefault(
                        sl => sl.LanguageCode.Equals(UserHelper.CustomerSession.Language, StringComparison.OrdinalIgnoreCase));

                return shippingMethodLanguage != null ? shippingMethodLanguage.DisplayName : Method.DisplayName ?? Method.Name;
            }
        }

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>The description.</value>
        public string Description
        {
            get { return Method.Description; }
        }

		/// <summary>
		/// Gets the price.
		/// </summary>
		/// <value>The price.</value>
        public decimal Price
        {
            get { return Rate != null ? Rate.Price : 0; }
        }

		/// <summary>
		/// Gets the currency.
		/// </summary>
		/// <value>The currency.</value>
        public string Currency
        {
            get { return Method.Currency; }
        }

		/// <summary>
		/// Gets the price formatted.
		/// </summary>
		/// <value>The price formatted.</value>
        public string PriceFormatted
        {
            get { return StoreHelper.FormatCurrency(Price, Currency); }
        }

		/// <summary>
		/// Gets the total cart price formatted.
		/// </summary>
		/// <value>The total cart price formatted.</value>
        public string TotalCartPriceFormatted
        {
            get { return StoreHelper.FormatCurrency(Cart.Total, Cart.BillingCurrency); }
        }

		/// <summary>
		/// Gets a value indicating whether this instance is current.
		/// </summary>
		/// <value><c>true</c> if this instance is current; otherwise, <c>false</c>.</value>
        public bool IsCurrent
        {
            get
            {
                return Cart.OrderForms.Any(f =>
                                           f.LineItems.Any(l => !string.IsNullOrEmpty(l.ShippingMethodId)
                                                                && string.Equals(l.ShippingMethodId, Id)));
            }
        }

		/// <summary>
		/// Gets or sets the rate.
		/// </summary>
		/// <value>The rate.</value>
        public ShippingRateModel Rate { get; set; }

		/// <summary>
		/// Gets or sets the cart.
		/// </summary>
		/// <value>The cart.</value>
        [JsonIgnore]
        public ShoppingCart Cart { get; set; }

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.</returns>
        public int CompareTo(ShippingMethodModel other)
        {
            return Price.CompareTo(other.Price);
        }
    }

	/// <summary>
	/// Class ShippingRateModel.
	/// </summary>
    public class ShippingRateModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ShippingRateModel"/> class.
		/// </summary>
		/// <param name="rate">The rate.</param>
        public ShippingRateModel(ShippingRate rate)
        {
            ShippingRate = rate;
        }

		/// <summary>
		/// Gets or sets the shipping rate.
		/// </summary>
		/// <value>The shipping rate.</value>
        public ShippingRate ShippingRate { get; set; }

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id
        {
            get { return ShippingRate.Id; }
        }

		/// <summary>
		/// Gets the display name.
		/// </summary>
		/// <value>The display name.</value>
        public string DisplayName
        {
            get { return ShippingRate.Name; }
        }

		/// <summary>
		/// Gets the price.
		/// </summary>
		/// <value>The price.</value>
        public decimal Price
        {
            get { return ShippingRate.Price; }
        }

		/// <summary>
		/// Gets the currency.
		/// </summary>
		/// <value>The currency.</value>
        public string Currency
        {
            get { return ShippingRate.CurrencyCode; }
        }

		/// <summary>
		/// Gets the price formatted.
		/// </summary>
		/// <value>The price formatted.</value>
        public string PriceFormatted
        {
            get { return StoreHelper.FormatCurrency(Price, Currency); }
        }
    }
}
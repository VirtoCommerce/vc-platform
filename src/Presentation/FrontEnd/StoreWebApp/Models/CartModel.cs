using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model.Countries;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class ShippingEstimateAddress.
	/// </summary>
    public class ShippingEstimateAddress
    {
		/// <summary>
		/// Gets or sets the country code.
		/// </summary>
		/// <value>The country code.</value>
        [Required]
		[Display(Name = "Country")]
        public string CountryCode { get; set; }

		/// <summary>
		/// Gets or sets the name of the country.
		/// </summary>
		/// <value>The name of the country.</value>
        public string CountryName { get; set; }
		/// <summary>
		/// Gets or sets the state province.
		/// </summary>
		/// <value>The state province.</value>
		[Display(Name = "State/Province")]
        public string StateProvince { get; set; }
		/// <summary>
		/// Gets or sets the postal code.
		/// </summary>
		/// <value>The postal code.</value>
		[Display(Name = "Zip/Postal Code")]
        public string PostalCode { get; set; }
    }

	/// <summary>
	/// Class ShippingEstimateModel.
	/// </summary>
    public class ShippingEstimateModel
    {
		/// <summary>
		/// The _regions
		/// </summary>
        private CountryRegionScriptModel[] _regions;

		/// <summary>
		/// Initializes a new instance of the <see cref="ShippingEstimateModel"/> class.
		/// </summary>
        public ShippingEstimateModel()
        {
            Address = new ShippingEstimateAddress();
        }

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
        public ShippingEstimateAddress Address { get; set; }

		/// <summary>
		/// Gets or sets the countries.
		/// </summary>
		/// <value>The countries.</value>
        public Country[] Countries { get; set; }

		/// <summary>
		/// Gets the script country regions.
		/// </summary>
		/// <value>The script country regions.</value>
        public CountryRegionScriptModel[] ScriptCountryRegions
        {
            get
            {
                if (_regions != null || Countries == null)
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
	/// Class CartModel.
	/// </summary>
    public class CartModel
    {
		/// <summary>
		/// The _shipping estimate model
		/// </summary>
        private ShippingEstimateModel _shippingEstimateModel = new ShippingEstimateModel();

		/// <summary>
		/// Gets or sets the shipping estimate model.
		/// </summary>
		/// <value>The shipping estimate model.</value>
        public ShippingEstimateModel ShippingEstimateModel
        {
            get { return _shippingEstimateModel; }
            set { _shippingEstimateModel = value; }
        }

		/// <summary>
		/// Gets or sets the line items.
		/// </summary>
		/// <value>The line items.</value>
        public LineItemModel[] LineItems { get; set; }

		/// <summary>
		/// Gets or sets the sub total price formatted.
		/// </summary>
		/// <value>The sub total price formatted.</value>
        public string SubTotalPriceFormatted { get; set; }

		/// <summary>
		/// Gets or sets the total price formatted.
		/// </summary>
		/// <value>The total price formatted.</value>
        public string TotalPriceFormatted { get; set; }

        /// <summary>
        /// Gets or sets the discount total price formatted.
        /// </summary>
        /// <value>
        /// The discount total price formatted.
        /// </value>
        public string DiscountTotalPriceFormatted { get; set; }

		/// <summary>
		/// Gets or sets the shipping price formatted.
		/// </summary>
		/// <value>The shipping price formatted.</value>
        public string ShippingPriceFormatted { get; set; }

		/// <summary>
		/// Gets or sets the tax total price formatted.
		/// </summary>
		/// <value>The tax total price formatted.</value>
		public string TaxTotalPriceFormatted { get; set; }

		/// <summary>
		/// Gets or sets the coupon code.
		/// </summary>
		/// <value>The coupon code.</value>
        public string CouponCode { get; set; }
    }
}
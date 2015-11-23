using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Cart
{
    public class Payment : Entity
    {
        /// <summary>
        /// Gets or sets the value of payment outer id
        /// </summary>
        public string OuterId { get; set; }

        /// <summary>
        /// Gets or sets the value of payment gateway code
        /// </summary>
        public string PaymentGatewayCode { get; set; }

        /// <summary>
        /// Gets or sets the value of payment currency
        /// </summary>
        /// <value>
        /// Currency code in ISO 4217 format
        /// </value>
        public CurrencyCodes Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of payment amount
        /// </summary>
        public Money Amount { get; set; }

        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        /// <value>
        /// Address object
        /// </value>
        public Address BillingAddress { get; set; }
    }
}
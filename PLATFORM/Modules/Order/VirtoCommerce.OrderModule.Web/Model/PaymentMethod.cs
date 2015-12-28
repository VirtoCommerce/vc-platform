using System.Collections.Generic;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.OrderModule.Web.Model
{
    public class PaymentMethod  : ValueObject<PaymentMethod>
    {
        /// <summary>
        /// Gets or sets the value of payment gateway code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method logo absolute URL
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method type
        /// </summary>
        /// <value>
        /// "Unknown", "Standard", "Redirection", "PreparedForm"
        /// </value>
        public PaymentMethodType PaymentMethodType { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method group type
        /// </summary>
        /// <value>
        /// "Paypal", "BankCard", "Alternative", "Manual"
        /// </value>
        public PaymentMethodGroupType PaymentMethodGroupType { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Is payment method available for partial payments
        /// </summary>
        public bool IsAvailableForPartial { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	public enum PaymentMethodGroupType
	{
		/// <summary>
		/// Paypal type, redirecting payer to paypal site
		/// </summary>
		Paypal,
		/// <summary>
		/// Bank card type, information about card is entered on our site
		/// </summary>
		BankCard,
		/// <summary>
		/// Alternative type, redirecting payer to external payment or using prepared html-form/iframe
		/// </summary>
		Alternative,
		/// <summary>
		/// Manual payment method type, when user pay out of web site by instruction (like Cash on Demand (COD), bank deposit)
		/// </summary>
		Manual
	}
}

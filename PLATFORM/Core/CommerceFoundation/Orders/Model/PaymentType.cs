using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Orders.Model
{
    [DataContract, Serializable]
	public enum PaymentType
	{
		/// <summary>
		/// Custom implementation.
		/// </summary>
		Other = 0,
		/// <summary>
		/// Typical credit card payment.
		/// </summary>
		CreditCard = 1,
		/// <summary>
		/// Cash card is a company issued card for making purchases.
		/// </summary>
		CashCard = 2,
		/// <summary>
		/// Typically business to business payment type.
		/// </summary>
		Invoice = 3,
		/// <summary>
		/// Gift card issued by the store.
		/// </summary>
		GiftCard =4,
		/// <summary>
		/// Exchange payment
		/// </summary>
		Exchange = 5
	}
}

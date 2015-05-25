using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	/// <summary>
	/// Represents a payment status enumeration
	/// </summary>
	public enum PaymentStatus
	{
		/// <summary>
		/// Pending
		/// </summary>
		Pending,
		/// <summary>
		/// Authorized
		/// </summary>
		Authorized,
		/// <summary>
		/// Paid
		/// </summary>
		Paid,
		/// <summary>
		/// Partially Refunded
		/// </summary>
		PartiallyRefunded,
		/// <summary>
		/// Refunded
		/// </summary>
		Refunded,
		/// <summary>
		/// Voided
		/// </summary>
		Voided
	}
}

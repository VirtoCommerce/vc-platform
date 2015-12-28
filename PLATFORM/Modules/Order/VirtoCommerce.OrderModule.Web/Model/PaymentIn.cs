using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.OrderModule.Web.Model
{
	/// <summary>
	/// Represent incoming payment operation 
	/// </summary>
	public class PaymentIn : Operation
	{
		/// <summary>
		/// Customer organization
		/// </summary>
		public string OrganizationName { get; set; }
		public string OrganizationId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerId { get; set; }

		/// <summary>
		/// Payment purpose text 
		/// </summary>
		public string Purpose { get; set; }

	    /// <summary>
        /// Payment method for current order payment
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

		/// <summary>
		/// Expected date of receipt of payment
		/// </summary>
		public DateTime? IncomingDate { get; set; }
		/// <summary>
		/// Outer id used for link with payment in external systems
		/// </summary>
		public string OuterId { get; set; }
	}
}
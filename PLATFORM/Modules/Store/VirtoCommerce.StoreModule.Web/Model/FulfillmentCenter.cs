using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Web.Model
{
	public class FulfillmentCenter  : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int MaxReleasesPerPickBatch { get; set; }
		public int PickDelay { get; set; }
		public string DaytimePhoneNumber { get; set; }

		/// <summary>
		/// Part of fulfillment center address, line1
		/// </summary>
		public string Line1 { get; set; }

		/// <summary>
		/// Part of fulfillment center address, line2
		/// </summary>
		public string Line2 { get; set; }

		/// <summary>
		/// Part of fulfillment center address, city
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Part of fulfillment center address, state province
		/// </summary>
		public string StateProvince { get; set; }

		/// <summary>
		/// Part of fulfillment center address, country code
		/// </summary>
		public string CountryCode { get; set; }

		/// <summary>
		/// Part of fulfillment center address, country name
		/// </summary>
		public string CountryName { get; set; }

		/// <summary>
		/// Part of fulfillment center address, postal code
		/// </summary>
		public string PostalCode { get; set; }
	}
}

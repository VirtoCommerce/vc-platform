using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.CoreModule.Data.Model
{
	public class FulfillmentCenter : AuditableEntity
	{
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		public int MaxReleasesPerPickBatch { get; set; }

		public int PickDelay { get; set; }

		[Required]
		[StringLength(32)]
		public string DaytimePhoneNumber { get; set; }

		[Required]
		[StringLength(128)]
		public string Line1 { get; set; }

		[StringLength(128)]
		public string Line2 { get; set; }

		[Required]
		[StringLength(128)]
		public string City { get; set; }

		[StringLength(128)]
		public string StateProvince { get; set; }

		[Required]
		[StringLength(64)]
		public string CountryCode { get; set; }

		[Required]
		[StringLength(128)]
		public string CountryName { get; set; }

		[Required]
		[StringLength(32)]
		public string PostalCode { get; set; }
	}
}

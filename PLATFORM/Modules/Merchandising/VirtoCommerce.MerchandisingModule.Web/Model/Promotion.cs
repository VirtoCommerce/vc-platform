using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public class Promotion : AuditableEntity
	{
		public string Type { get; set; }
		public string Name { get; set; }
		public string Store { get; set; }
		public string Catalog { get; set; }

		public string Description { get; set; }
		public bool IsActive { get; set; }
		public int MaxUsageCount { get; set; }
		public int MaxPersonalUsageCount { get; set; }
		public string[] Coupons { get; set; }

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
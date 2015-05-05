using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Marketing.Model
{

	public class PromotionUsage : AuditableEntity
	{
		public string CustomerId { get; set; }
		public string CustomerName { get; set; }

		public int UsageCount { get; set; }

		public int PromotionId { get; set; }

		public Promotion Promotion { get; set; }

	}
}

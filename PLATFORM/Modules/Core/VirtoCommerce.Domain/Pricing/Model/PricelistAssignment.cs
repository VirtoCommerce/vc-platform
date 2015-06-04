using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class PricelistAssignment : AuditableEntity
	{
		public string CatalogId { get; set; }
		public string PricelistId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string ConditionExpression { get; set; }
		public string  PredicateVisualTreeSerialized { get; set; }
	}
}

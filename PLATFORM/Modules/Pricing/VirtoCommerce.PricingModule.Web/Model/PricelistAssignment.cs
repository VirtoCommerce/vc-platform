using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.PricingModule.Web.Model
{
	public class PricelistAssignment : AuditableEntity
	{
		public string CatalogName { get; set; }
		public string CatalogId { get; set; }
		public string PricelistId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public ConditionExpressionTree DynamicExpression { get; set; }
	}
}
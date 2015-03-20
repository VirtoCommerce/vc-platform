using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.PricingModule.Web.Model
{
	public class PricelistAssignment : Entity
	{
		public string CatalogId { get; set; }
		public string PriceListId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Priority { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string ConditionExpression { get; set; }
	}
}
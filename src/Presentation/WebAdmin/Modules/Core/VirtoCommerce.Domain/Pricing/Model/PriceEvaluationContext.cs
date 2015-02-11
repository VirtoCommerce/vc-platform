using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class PriceEvaluationContext : ValueObject<PriceEvaluationContext>
	{
		public string StoreId { get; set; } 
		public string CatalogId { get; set; }
		public string ProductId { get; set; }
		public string CustomerId { get; set; }
		public string OrganizationId { get; set; }
		public DateTime? CertainDate { get; set; }
	}
}

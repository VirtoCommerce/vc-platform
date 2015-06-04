using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Pricing.Model
{
	public class Pricelist : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public CurrencyCodes Currency { get; set; }
		public ICollection<Price> Prices { get; set; }
		public ICollection<PricelistAssignment> Assignments { get; set; }
	}
}

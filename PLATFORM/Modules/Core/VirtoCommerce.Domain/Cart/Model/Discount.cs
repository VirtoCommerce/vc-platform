using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Cart.Model
{
	public class Discount : Entity
	{
		public string PromotionId { get; set; }
		public CurrencyCodes Currency { get; set; }
		public decimal DiscountAmount { get; set; }

		public string Description { get; set; }
	}
}

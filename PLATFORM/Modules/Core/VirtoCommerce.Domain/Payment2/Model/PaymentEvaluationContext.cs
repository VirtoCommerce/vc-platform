using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Payment2.Model
{
	public class PaymentEvaluationContext : IEvaluationContext
	{
		public PaymentEvaluationContext(string storeId)
		{
			StoreId = storeId;
		}
		public string StoreId { get; set; }

		public CurrencyCodes Currency { get; set; }

		public ShoppingCart ShoppingCart { get; set; }
	}
}

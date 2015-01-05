using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public class PaymentIn : FinanceInOperation
	{
		public DateTime? IncomingDate { get; set; }
		public string IncomingNumber { get; set; }
	}
}

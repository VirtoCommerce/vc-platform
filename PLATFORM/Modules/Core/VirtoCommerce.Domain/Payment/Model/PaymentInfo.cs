using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class PaymentInfo
	{
		public string Id { get; set; }
		public string GatewayCode { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string Status { get; set; }
		public bool IsApproved { get; set; }
		public CurrencyCodes Currency { get; set; }
		public decimal Amount { get; set; }
		public string Error { get; set; }
	}
}

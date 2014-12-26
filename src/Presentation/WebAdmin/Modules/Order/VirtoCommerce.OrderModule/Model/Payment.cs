using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Model
{
	public class Payment : AuditableEntityBase<Payment>, INumbered
	{
		public Payment(string number)
		{
			Number = number;
		}

		#region INumbered Members
		public string Number
		{
			get;
			set;
		}
		#endregion

		public string OuterId { get; set; }

		public string PaymentGatewayCode { get; set; }

		public bool  IsActive { get; set; }

		public Money Balance
		{
			get
			{
				return AmountCollected - AmountRequested;
			}
		}

		public Money AmountRequested { get; set; }
		public Money AmountCollected { get; set; }
	}
}

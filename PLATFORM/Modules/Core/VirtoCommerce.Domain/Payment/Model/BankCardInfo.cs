using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class BankCardInfo
	{
		public string BankCardNumber { get; set; }
		public string BankCardType { get; set; }
		public int BankCardMonth { get; set; }
		public int BankCardYear { get; set; }
		public string BankCardCVV2 { get; set; }
	}
}

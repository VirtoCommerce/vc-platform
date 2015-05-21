using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	public class DirectRedirectUrlPaymentInfo : PaymentInfo
	{
		public string RedirectUrl { get; set; }
	}
}

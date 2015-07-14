using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Model
{
	public interface IProcessResult
	{
		bool IsSuccess { get; set; }
		string ErrorMessage { get; set; }
		PaymentStatus NewPaymentStatus { get; set; }
	}
}

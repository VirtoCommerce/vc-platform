using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class PaymentIn : OperationTreeNode
	{
		public PaymentIn()
		{
			OperationType = "PaymentIn";
		}

		public string PaymentPurpose { get; set; }

		public string PaymentGatewayCode { get; set; }

		public DateTime? IncomingDate { get; set; }
		public string OuterId { get; set; }
	}
}
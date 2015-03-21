using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;

namespace VirtoCommerce.Foundation.Orders.CQRS.Messages
{
	[DataContract]
	public class SavePaymentMethodChangesMessage : IMessage
	{
		[DataMember]
		public PaymentMethod PaymentMethod
		{
			get;
			set;
		}

		public SavePaymentMethodChangesMessage(PaymentMethod paymentMethod)
		{
			PaymentMethod = paymentMethod;
		}

	}
}

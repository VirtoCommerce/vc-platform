using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Frameworks.CQRS;

namespace VirtoCommerce.Foundation.Orders.CQRS.Messages
{
	[DataContract]
	public class SaveOrderGroupChangesMessage : IMessage
	{
		[DataMember]
		public OrderGroup OrderGroup
		{
			get; 
			set; 
		}

		public SaveOrderGroupChangesMessage(OrderGroup order)
		{
			OrderGroup = order;
		}
	
	}
}

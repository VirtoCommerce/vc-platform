using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Orders.CQRS.Messages;
using VirtoCommerce.Foundation.Frameworks.CQRS;

namespace VirtoCommerce.Foundation.Orders.CQRS.Handlers
{
	[MessageType(typeof(SaveOrderGroupChangesMessage))]
	public class RebuildIndexHandler : IConsume
	{
		#region IConsume<SaveOrderChangesMesssage> Members

		public void Consume(IMessage message)
		{
			//Do somthing
		}

		#endregion
	}
}

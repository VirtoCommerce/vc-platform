using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.OrderModule.Data.Workflow
{
	public class CustomerOrderStateBasedEvalContext
	{
		public CustomerOrderStateBasedEvalContext(EntryState state, CustomerOrder origOrder, CustomerOrder modifiedOrder)
		{
			ChangeState = state;
			OrigOrder = origOrder;
			ModifiedOrder = modifiedOrder;
		}

		public EntryState ChangeState { get; set; }
		public CustomerOrder OrigOrder { get; set; }
		public CustomerOrder ModifiedOrder { get; set; }
	}
}

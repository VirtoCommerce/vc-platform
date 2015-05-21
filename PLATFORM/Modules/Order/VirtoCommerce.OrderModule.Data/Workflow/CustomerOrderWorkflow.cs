using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Order.Workflow;
using VirtoCommerce.OrderModule.Data.Workflow;

namespace VirtoCommerce.OrderModule.Data.Workflow
{
	public class CustomerOrderWorkflow : ObservableWorkflowService<OrderStateBasedEvalContext>, IOrderWorkflow
	{

	}
}

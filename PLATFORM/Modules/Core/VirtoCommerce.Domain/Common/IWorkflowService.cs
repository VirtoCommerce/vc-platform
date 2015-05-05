using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Domain.Common
{
	public interface IWorkflowService
	{
		void RunWorkflow(object context);
	}
}

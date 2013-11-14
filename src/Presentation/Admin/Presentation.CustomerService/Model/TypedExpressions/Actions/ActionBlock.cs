using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
    [Serializable]
    public class ActionBlock : TypedExpressionElementBase
    {
        public ActionBlock(string name, IExpressionViewModel caseRuleViewModel)
			: base(name, caseRuleViewModel)
        {
            WithLabel(name);
        }

		public CaseAlert[] GetCaseAlerts()
		{
			List<CaseAlert> retVal = new List<CaseAlert>();
			foreach (var adaptor in this.Children.OfType<IExpressionCaseAlertsAdaptor>())
			{
				retVal.AddRange(adaptor.GetCaseAlerts());
			}
			return retVal.ToArray();
		}
    }
}

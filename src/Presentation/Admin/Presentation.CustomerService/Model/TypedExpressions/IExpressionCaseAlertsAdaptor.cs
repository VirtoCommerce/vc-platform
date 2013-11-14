using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
	public interface IExpressionCaseAlertsAdaptor: IExpressionAdaptor
	{
		CaseAlert[] GetCaseAlerts();
	}
}

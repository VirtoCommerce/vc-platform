using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.Foundation.Customers.Services
{
	public interface ICaseAlertsEvaluator
	{
		CaseAlert[] Evaluate(ICaseAlertEvaluationContext context);
	}
} 

using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Customers.Services
{
	public interface ICaseAlertEvaluationContext: IEvaluationContext
	{  
		Case CurrentCase { get; set; }
	}
}

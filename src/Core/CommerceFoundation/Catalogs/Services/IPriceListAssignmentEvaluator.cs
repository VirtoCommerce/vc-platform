using VirtoCommerce.Foundation.Catalogs.Model;
namespace VirtoCommerce.Foundation.Catalogs.Services
{
	public interface IPriceListAssignmentEvaluator
	{
		Pricelist[] Evaluate(IPriceListAssignmentEvaluationContext context);
	}
}

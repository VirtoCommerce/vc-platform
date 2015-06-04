namespace VirtoCommerce.Foundation.AppConfig.Model
{
	public interface IDisplayTemplateEvaluator
	{
		string Evaluate(IDisplayTemplateEvaluationContext context);
	}
}

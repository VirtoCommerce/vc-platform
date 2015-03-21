namespace VirtoCommerce.Foundation.Marketing.Model
{
	public interface IPromotionEvaluator
	{
		Promotion[] EvaluatePromotion(IPromotionEvaluationContext context);
	    PromotionRecord[] EvaluatePolicies(PromotionRecord[] records, IEvaluationPolicy[] policies = null);
	}
}

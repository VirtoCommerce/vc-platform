using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	public interface IExpressionMarketingAdaptor: IExpressionAdaptor
	{
		PromotionReward[] GetPromotionRewards();
	}
}

using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class PromotionRewardConverter
	{
		public static webModel.PromotionReward ToWebModel(this coreModel.PromotionReward reward)
		{
			var retVal = new webModel.PromotionReward();
			retVal.InjectFrom(reward);
			retVal.RewardType = reward.GetType().Name;
			if (reward.Promotion != null)
			{
				retVal.Promotion = reward.Promotion.ToWebModel();
			}
			return retVal;
		}
	
	}
}

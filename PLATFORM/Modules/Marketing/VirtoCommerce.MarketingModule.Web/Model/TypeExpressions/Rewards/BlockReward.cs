using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using coreModel =  VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	public class RewardBlock : DynamicBlockExpression, IRewardExpression
	{
		#region IRewardsExpression Members

		public coreModel.PromotionReward[] GetRewards()
		{
			var retVal = new coreModel.PromotionReward[] { };
			if (Children != null)
			{
				 retVal = Children.OfType<IRewardExpression>().SelectMany(x => x.GetRewards()).ToArray();
			}
			return retVal;
		}

		#endregion
	}
}
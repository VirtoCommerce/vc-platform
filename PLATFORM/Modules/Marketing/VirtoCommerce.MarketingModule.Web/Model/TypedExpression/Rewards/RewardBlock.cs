using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Conditions
{
	public class RewardBlock : CompositeElement, IRewardExpression
	{
		#region IRewardsExpression Members

		public Domain.Marketing.Model.PromotionReward[] GetRewards()
		{
			var retVal = Children.OfType<IRewardExpression>().SelectMany(x => x.GetRewards()).ToArray();

			return retVal;
		}

		#endregion
	}
}
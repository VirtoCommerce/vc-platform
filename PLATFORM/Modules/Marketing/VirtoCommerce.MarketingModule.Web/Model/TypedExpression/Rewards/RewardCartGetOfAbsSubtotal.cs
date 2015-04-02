using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Actions
{
	public class RewardCartGetOfAbsSubtotal : CompositeElement, IRewardExpression
	{
		public decimal Amount { get; set; }

		#region IRewardsExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CartSubtotalReward()
			{
				IsValid = true,
				Amount = Amount
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}
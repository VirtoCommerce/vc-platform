using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Get [] $ off cart subtotal
	public class RewardCartGetOfAbsSubtotal : DynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }

		#region IRewardsExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CartSubtotalReward()
			{
				Amount = Amount
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}
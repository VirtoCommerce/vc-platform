using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.DynamicExpressionModule.Data.Promotion
{
	//Get [] % off cart subtotal
	public class RewardCartGetOfRelSubtotal : DynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }

		#region IRewardsExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}
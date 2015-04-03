using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get [] % off cart subtotal
	public class RewardCartGetOfRelSubtotal : PromoDynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }

		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CartSubtotalReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}
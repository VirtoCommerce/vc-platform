using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get [] % off cart subtotal
	public class RewardCartGetOfRelSubtotal : DynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }

		#region IRewardExpression Members

		public coreModel.PromotionReward[] GetRewards()
		{
			var retVal = new CartSubtotalReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative
			};
			return new coreModel.PromotionReward[] { retVal };
		}

		#endregion
	}
}
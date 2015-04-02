using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get []% off 
	public class RewardItemGetOfRel : DynamicExpressionBase, IRewardExpression
	{
		public decimal Amount { get; set; }
		public string ProductId { get; set; }

		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative,
				ProductId = ProductId
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

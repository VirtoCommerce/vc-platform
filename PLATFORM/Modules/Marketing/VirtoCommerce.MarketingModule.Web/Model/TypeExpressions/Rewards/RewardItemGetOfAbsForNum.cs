using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get $[] off [] items
	public class RewardItemGetOfAbsForNum : DynamicExpressionBase, IRewardExpression
	{
		public decimal Amount { get; set; }
		public string ProductId { get; set; }
		public int NumItem { get; set; }
	
		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Absolute,
				QuantityLimit = NumItem,
				ProductId = ProductId
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

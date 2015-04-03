using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get $[] off 
	public class RewardItemGetOfAbs : DynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }
		public string ProductId { get; set; }
		public string CategoryId { get; set; }
		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Absolute,
				ProductId = ProductId
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

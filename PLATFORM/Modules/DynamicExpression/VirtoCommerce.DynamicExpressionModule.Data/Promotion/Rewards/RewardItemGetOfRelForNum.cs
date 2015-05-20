using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.DynamicExpressionModule.Data.Promotion
{
    //Get []% off [] items of entry []
	public class RewardItemGetOfRelForNum : DynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }
		public string ProductId { get; set; }
		public int NumItem { get; set; }
		public string ProductName { get; set; }
		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative,
				Quantity = NumItem,
				ProductId = ProductId
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

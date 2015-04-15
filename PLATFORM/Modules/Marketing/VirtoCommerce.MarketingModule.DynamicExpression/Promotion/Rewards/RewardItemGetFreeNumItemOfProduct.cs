using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Get [] free items of Product []
	public class RewardItemGetFreeNumItemOfProduct : DynamicExpression, IRewardExpression
	{
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		public int NumItem { get; set; }
		
		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = 100,
				AmountType = RewardAmountType.Relative,
				Quantity = NumItem,
				ProductId = ProductId
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

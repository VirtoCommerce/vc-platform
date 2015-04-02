using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get [] free items of Product []
	public class RewardItemGetFreeNumItemOfProduct : DynamicExpressionBase, IRewardExpression
	{
		public string ProductId { get; set; }
		public int NumItem { get; set; }
		
		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = 100,
				AmountType = RewardAmountType.Relative,
				QuantityLimit = NumItem,
				ProductId = ProductId
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

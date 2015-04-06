using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get [] free items of Product []
	public class RewardItemGetFreeNumItemOfProduct : DynamicExpression, IRewardExpression
	{
		public string ProductId { get; set; }
		public int NumItem { get; set; }
		public string CategoryId { get; set; }
		
		#region IRewardExpression Members

		public coreModel.PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = 100,
				AmountType = RewardAmountType.Relative,
				Quantity = NumItem,
				ProductId = ProductId
			};
			return new coreModel.PromotionReward[] { retVal };
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get []% off [] items
	public class RewardItemGetOfRelForNum : DynamicExpression, IRewardExpression
	{
		public decimal Amount { get; set; }
		public string ProductId { get; set; }
		public int NumItem { get; set; }
		public string CategoryId { get; set; }
		#region IRewardExpression Members

		public coreModel.PromotionReward[] GetRewards()
		{
			var retVal = new CatalogItemAmountReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative,
				Quantity = NumItem,
				ProductId = ProductId
			};
			return new coreModel.PromotionReward[] { retVal };
		}

		#endregion
	}
}

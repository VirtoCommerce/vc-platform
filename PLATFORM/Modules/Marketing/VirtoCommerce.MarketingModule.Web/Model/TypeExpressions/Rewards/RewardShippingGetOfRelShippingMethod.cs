using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get $[] off shipping
	public class RewardShippingGetOfRelShippingMethod : DynamicExpression, IRewardExpression
	{

		public decimal Amount { get; set; }
		public string ShippingMethod { get; set; }

	
		#region IRewardExpression Members

		public coreModel.PromotionReward[] GetRewards()
		{
			var retVal = new ShipmentReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative,
				ShippingMethod = ShippingMethod
			};
			return new coreModel.PromotionReward[] { retVal };
		}

		#endregion
	}
}
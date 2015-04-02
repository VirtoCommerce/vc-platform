using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get $[] off shipping
	public class RewardShippingGetOfRelShippingMethod : DynamicExpressionBase, IRewardExpression
	{

		public decimal Amount { get; set; }
		public string ShippingMethod { get; set; }

	
		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new ShipmentReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Relative,
				ShippingMethod = ShippingMethod
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}
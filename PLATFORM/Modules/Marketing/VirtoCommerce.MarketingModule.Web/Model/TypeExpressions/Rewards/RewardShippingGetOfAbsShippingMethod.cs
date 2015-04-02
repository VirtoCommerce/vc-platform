using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Get $[] off shipping
	public class RewardShippingGetOfAbsShippingMethod : DynamicExpressionBase, IRewardExpression
	{

		public decimal Amount { get; set; }
		public string ShippingMethod { get; set; }

		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new ShipmentReward
			{
				Amount = Amount,
				AmountType = RewardAmountType.Absolute,
				ShippingMethod = ShippingMethod
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}
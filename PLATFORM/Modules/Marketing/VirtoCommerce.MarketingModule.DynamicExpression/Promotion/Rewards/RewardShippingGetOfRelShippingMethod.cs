using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Get $[] off shipping
	public class RewardShippingGetOfRelShippingMethod : DynamicExpression, IRewardExpression
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
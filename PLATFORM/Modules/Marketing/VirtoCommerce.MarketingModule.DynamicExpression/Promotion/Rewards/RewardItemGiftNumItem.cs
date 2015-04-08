using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.DynamicExpression.Promotion
{
	//Gift [] of Product []
	public class RewardItemGiftNumItem : DynamicExpression, IRewardExpression
	{
		public string Name { get; set; }
		public string CategoryId { get; set; }
		public string ProductId { get; set; }
		public int Quantity { get; set; }
		public string MeasureUnit { get; set; }
		public string ImageUrl { get; set; }
		public string Description { get; set; }

		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new GiftReward
			{
				Name = Name,
				CategoryId = CategoryId,
				ProductId = ProductId,
				Quantity = Quantity,
				MeasureUnit = MeasureUnit,
		        ImageUrl = ImageUrl,
				Description = Description
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions
{
	//Gift [] of Product []
	public class RewardItemGiftNumItem : DynamicExpression, IRewardExpression
	{
		public string Name { get; set; }
		public string CategoryId { get; set; }
		public string SellerId { get; set; }
		public string ProductId { get; set; }
		public int Quantity { get; set; }
		public string Unit { get; set; }
		public string HtmlInformer { get; set; }
		public string ThumbImageUrl { get; set; }
		public string OriginalImageUrl { get; set; }
		public string Description { get; set; }

		#region IRewardExpression Members

		public PromotionReward[] GetRewards()
		{
			var retVal = new GiftCatalogItemReward
			{
				Name = Name,
				CategoryId = CategoryId,
				SellerId = SellerId,
				ProductId = ProductId,
				Quantity = Quantity,
				Unit = Unit,
				HtmlInformer = HtmlInformer,
				ThumbImageUrl = ThumbImageUrl,
				OriginalImageUrl = OriginalImageUrl,
				Description = Description
			};
			return new PromotionReward[] { retVal };
		}

		#endregion
	}
}

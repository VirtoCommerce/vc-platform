using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;

namespace VirtoCommerce.MarketingModule.Test.CustomPromotion
{
	public class BuyProductWithTagPromotion : Promotion
	{
		private readonly string[] _tags;
		private readonly decimal _discountAmount;

		public BuyProductWithTagPromotion(string[] tags, decimal discountAmount)
		{
			_tags = tags;
			_discountAmount = discountAmount;
		}

		public override PromotionReward[] EvaluatePromotion(IEvaluationContext context)
		{
			var retVal = new List<PromotionReward>();
			var promoContext = context as PromotionEvaluationContext;
			if (promoContext != null)
			{
				foreach (var entry in promoContext.PromoEntries)
				{
					var tag = entry.Attributes != null ? entry.Attributes["tag"] : null;
					var reward = new CatalogItemAmountReward
					{
						AmountType = RewardAmountType.Relative,
						Amount = _discountAmount,
						IsValid = !String.IsNullOrEmpty(tag) ? _tags.Contains(tag) : false,
						ProductId = entry.ProductId,
						Promotion = this
					};
				}
			}
			return retVal.ToArray();
		}
	}
}

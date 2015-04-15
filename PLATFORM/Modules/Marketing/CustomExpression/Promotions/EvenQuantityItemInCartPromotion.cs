using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;

namespace CustomExpression
{
	/// <summary>
	/// Get discount for all items with even quantity 
	///  </summary>
	public class EvenNumberItemInCartPromotion : Promotion
	{
		private readonly decimal _discountAmount;

		public EvenNumberItemInCartPromotion(decimal discountAmount)
		{
			_discountAmount = discountAmount;
		}

		public override PromotionReward[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
			var retVal = new List<PromotionReward>();
			var promoContext = context as PromotionEvaluationContext;
			if (promoContext != null)
			{
				foreach (var entry in promoContext.PromoEntries)
				{
					var reward = new CatalogItemAmountReward
					{
						AmountType = RewardAmountType.Relative,
						Amount = _discountAmount,
						IsValid = entry.Quantity % 2 == 0,
						ProductId = entry.ProductId,
						Promotion = this
					};
				}
			}
			return retVal.ToArray();
		}
	}
}

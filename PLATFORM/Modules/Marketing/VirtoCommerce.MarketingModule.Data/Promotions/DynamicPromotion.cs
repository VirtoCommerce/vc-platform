using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExpressionSerialization;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Common;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
	public class DynamicPromotion : Promotion
	{
		public string PredicateSerialized { get; set; }
		public string PredicateVisualTreeSerialized { get; set; }
		public string RewardsSerialized { get; set; }
		public string Coupon { get; set; }

		public override PromotionReward[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
			var retVal = new List<PromotionReward>();
			var promoContext = context as PromotionEvaluationContext;
			if (promoContext == null)
			{
				throw new ArgumentException("context should be PromotionEvaluationContext");
			}

			//Check coupon
			var	isValid = !String.IsNullOrEmpty(Coupon) ? String.Equals(Coupon, promoContext.Coupon, StringComparison.InvariantCultureIgnoreCase) : true;
			
			//Check dynamic condition
			if (isValid)
			{
				var condition = SerializationUtil.DeserializeExpression<Func<IPromotionEvaluationContext, bool>>(PredicateSerialized);
				isValid = condition != null && condition(context);
			}

			//rewards
			if (!String.IsNullOrEmpty(RewardsSerialized))
			{
				var rewards = JsonConvert.DeserializeObject<PromotionReward[]>(RewardsSerialized, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
				foreach (var reward in rewards)
				{
					reward.Promotion = this;
					reward.IsValid = isValid;

					//Replace some reward types
					var cartSubtotalReward = reward as CartSubtotalReward;
					if (cartSubtotalReward != null && cartSubtotalReward.AmountType == RewardAmountType.Relative)
					{
						//Convert cart subtotal relative reward to line item relative rewards
						foreach (var promoEntry in promoContext.ProductPromoEntries)
						{
							if (!(promoEntry.Discount > 0))
							{
								var newReward = new CatalogItemAmountReward()
								{
									Promotion = this,
									Amount = cartSubtotalReward.Amount,
									AmountType = RewardAmountType.Relative,
									ProductId = promoEntry.ProductId,
									CategoryId = promoEntry.CategoryId,
									IsValid = isValid
								};
								retVal.Add(newReward);
							}
						}
					}
					else
					{
						retVal.Add(reward);
					}
				}
			}
			return retVal.ToArray();
		}

		public override PromotionReward[] ProcessEvent(IMarketingEvent marketingEvent)
		{
			return null;
		}

	}
}

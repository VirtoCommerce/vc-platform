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

		public override PromotionReward[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
			var retVal = new List<PromotionReward>();
			var promoContext = context as PromotionEvaluationContext;
			if (promoContext == null)
			{
				throw new ArgumentException("context should be PromotionEvaluationContext");
			}

			//Check coupon
			var couponValid = !String.IsNullOrEmpty(Coupon) ? String.Equals(Coupon, promoContext.Coupon, StringComparison.InvariantCultureIgnoreCase) : true;

			//deserealize dynamic condition
			var condition = SerializationUtil.DeserializeExpression<Func<IPromotionEvaluationContext, bool>>(PredicateSerialized);
			//deserealize rewards
			var rewards = JsonConvert.DeserializeObject<PromotionReward[]>(RewardsSerialized, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
			//Evaluate reward for all promoEntry in context
			foreach (var promoEntry in promoContext.PromoEntries)
			{
				//Set current context promo entry for evaluation
				promoContext.PromoEntry = promoEntry;
				foreach (var reward in rewards.Select(x=>x.Clone()))
				{
					reward.Promotion = this;
					reward.IsValid = couponValid && condition(promoContext);
					var catalogItemReward = reward as CatalogItemAmountReward;
					//Set productId for catalog item reward
					if (catalogItemReward != null && catalogItemReward.ProductId == null)
					{
						catalogItemReward.ProductId = promoEntry.ProductId;
					}
					retVal.Add(reward);
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

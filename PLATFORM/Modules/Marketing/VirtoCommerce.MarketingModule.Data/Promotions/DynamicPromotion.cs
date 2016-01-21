using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExpressionSerialization;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Data.Common;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
	public class DynamicPromotion : Promotion
	{
		public string PredicateSerialized { get; set; }
		public string PredicateVisualTreeSerialized { get; set; }
		public string RewardsSerialized { get; set; }

        private Func<IEvaluationContext, bool> _condition;
        private Func<IEvaluationContext, bool> Condition
        {
            get
            {
                if (_condition == null)
                {
                    //deserealize dynamic condition
                    _condition = SerializationUtil.DeserializeExpression<Func<IEvaluationContext, bool>>(PredicateSerialized);
                }
                return _condition;
            }
        }
        private PromotionReward[] _rewards;
        private PromotionReward[] Rewards
        {
            get
            {
                if (_rewards == null)
                {
                    //deserealize rewards
                    _rewards = JsonConvert.DeserializeObject<PromotionReward[]>(RewardsSerialized, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                }
                return _rewards;
            }
        }

		public override PromotionReward[] EvaluatePromotion(IEvaluationContext context)
		{
			var retVal = new List<PromotionReward>();
			var promoContext = context as PromotionEvaluationContext;
			if (promoContext == null)
			{
				throw new ArgumentException("context should be PromotionEvaluationContext");
			}

			//Check coupon
			var couponValid = (Coupons != null && Coupons.Any()) ? Coupons.Any(x=> String.Equals(x, promoContext.Coupon, StringComparison.InvariantCultureIgnoreCase)) : true;

		
			//Evaluate reward for all promoEntry in context
			foreach (var promoEntry in promoContext.PromoEntries)
			{
				//Set current context promo entry for evaluation
				promoContext.PromoEntry = promoEntry;
				foreach (var reward in Rewards.Select(x=>x.Clone()))
				{
					reward.Promotion = this;
					reward.IsValid = couponValid && Condition(promoContext);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExpressionSerialization;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Common;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
	public class DynamicPromotion : Promotion
	{
		public DynamicPromotion()
		{
		}

		public string PredicateSerialized { get; set; }
		public string PredicateVisualTreeSerialized { get; set; }
		public string RewardSerialized { get; set; }

		public override PromotionReward[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
			PromotionReward[]  retVal = null;
			var condition = SerializationUtil.DeserializeExpression<Func<IPromotionEvaluationContext, bool>>(PredicateSerialized);
			if (condition(context))
			{
				//var rewardExpression = DeserializeExpression<Func<IPromotionEvaluationContext, PromotionReward[]>>(RewardSerialized);
				//retVal = rewardExpression(context);
			}
			return retVal;
		}

		public override PromotionReward[] ProcessEvent(MarketingEvent marketingEvent)
		{
			return null;
		}

		
	}
}

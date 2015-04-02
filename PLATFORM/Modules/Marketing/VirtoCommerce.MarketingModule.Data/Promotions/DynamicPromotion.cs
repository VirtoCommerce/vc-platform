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
		public DynamicPromotion()
		{
		}

		public string PredicateSerialized { get; set; }
		public string PredicateVisualTreeSerialized { get; set; }
		public string RewardsSerialized { get; set; }

		public override PromotionReward[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
			PromotionReward[]  retVal = null;

			var condition = SerializationUtil.DeserializeExpression<Func<IPromotionEvaluationContext, bool>>(PredicateSerialized);
			if (condition(context) && !String.IsNullOrEmpty(RewardsSerialized))
			{
				retVal = JsonConvert.DeserializeObject<PromotionReward[]>(RewardsSerialized, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
			    foreach(var reward in retVal)
				{
					reward.Promotion = this;
				}
			}

			return retVal;
		}

		public override PromotionReward[] ProcessEvent(MarketingEvent marketingEvent)
		{
			return null;
		}

		
	}
}

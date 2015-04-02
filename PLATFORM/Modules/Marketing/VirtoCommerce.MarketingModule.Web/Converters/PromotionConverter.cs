using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ExpressionSerialization;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.MarketingModule.Data.Common;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class PromotionConverter
	{
		public static webModel.Promotion ToWebModel(this coreModel.Promotion promotion, DynamicPromotionExpression dynamicExpression = null)
		{
			var retVal = new webModel.Promotion();
			retVal.InjectFrom(promotion);
			var dynamicPromotion = promotion as DynamicPromotion;
			if (dynamicPromotion != null && dynamicExpression != null)
			{
				retVal.DynamicExpression = dynamicExpression;
				if (!String.IsNullOrEmpty(dynamicPromotion.PredicateVisualTreeSerialized))
				{
					retVal.DynamicExpression = JsonConvert.DeserializeObject<DynamicPromotionExpression>(dynamicPromotion.PredicateVisualTreeSerialized);
				}
			}
			return retVal;
		}

		public static coreModel.Promotion ToCoreModel(this webModel.Promotion promotion)
		{
			var retVal = new DynamicPromotion();
			retVal.InjectFrom(promotion);

			if (promotion.DynamicExpression != null)
			{
				var conditionExpression = promotion.DynamicExpression.GetConditionExpression();
				retVal.PredicateSerialized = SerializationUtil.SerializeExpression(conditionExpression);
				var rewards = promotion.DynamicExpression.GetRewards();
				retVal.RewardsSerialized = JsonConvert.SerializeObject(rewards, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
				retVal.PredicateVisualTreeSerialized = JsonConvert.SerializeObject(promotion.DynamicExpression);

			}
			return retVal;
		}
	}
}

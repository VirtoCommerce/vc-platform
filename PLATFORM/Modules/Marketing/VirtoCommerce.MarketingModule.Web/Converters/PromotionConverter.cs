using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ExpressionSerialization;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.MarketingModule.Data.Common;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Web.Model.TypedExpression;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class PromotionConverter
	{
		public static webModel.Promotion ToWebModel(this coreModel.Promotion promotion, CompositeElement dynamicExpression = null)
		{
			var retVal = new webModel.Promotion();
			retVal.InjectFrom(promotion);
			var dynamicPromotion = promotion as DynamicPromotion;
			if (dynamicPromotion != null && dynamicExpression != null)
			{
				retVal.DynamicExpression = dynamicExpression;
				if (!String.IsNullOrEmpty(dynamicPromotion.PredicateVisualTreeSerialized))
				{
					retVal.DynamicExpression = SerializationUtil.Deserialize<CompositeElement>(dynamicPromotion.PredicateVisualTreeSerialized);
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
				var dynamicPromotionBlock = promotion.DynamicExpression as DynamicPromotionBlock;
				if (dynamicPromotionBlock != null)
				{
					var conditionExpression = dynamicPromotionBlock.GetConditionExpression();
					retVal.PredicateSerialized = SerializationUtil.SerializeExpression(conditionExpression);
					var rewards = dynamicPromotionBlock.GetRewards();
					retVal.RewardSerialized = SerializationUtil.Serialize(rewards);
					retVal.PredicateVisualTreeSerialized = SerializationUtil.Serialize(promotion.DynamicExpression);
				}
			}
			return retVal;
		}
	}
}

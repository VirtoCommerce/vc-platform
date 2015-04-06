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
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class PromotionConverter
	{
		public static webModel.Promotion ToWebModel(this coreModel.Promotion promotion, PromoDynamicExpression dynamicExpression = null)
		{
			var retVal = new webModel.Promotion();
			retVal.InjectFrom(promotion);
			retVal.Type = promotion.GetType().Name;
			var dynamicPromotion = promotion as DynamicPromotion;
			if (dynamicPromotion != null && dynamicExpression != null)
			{
				retVal.DynamicExpression = dynamicExpression;
				if (!String.IsNullOrEmpty(dynamicPromotion.PredicateVisualTreeSerialized))
				{
					retVal.DynamicExpression = JsonConvert.DeserializeObject<PromoDynamicExpression>(dynamicPromotion.PredicateVisualTreeSerialized);
					//Add fresh available elements because it may be changed since last modifying
					var sourceBlocks = ((DynamicBlockExpression)dynamicExpression).Traverse(x => x.AvailableChildren != null ? x.AvailableChildren.OfType<DynamicBlockExpression>() : null);
					var targetBlocks = ((DynamicBlockExpression)retVal.DynamicExpression).Traverse(x => x.Children != null ? x.Children.OfType<DynamicBlockExpression>() : null);
					foreach (var sourceBlock in sourceBlocks)
					{
						foreach(var targetBlock in  targetBlocks.Where(x => x.Id == sourceBlock.Id))
						{
							targetBlock.AvailableChildren = sourceBlock.AvailableChildren;
						}
					}
				}
			}
			return retVal;
		}

		public static coreModel.Promotion ToCoreModel(this webModel.Promotion promotion)
		{
			var retVal = new DynamicPromotion();
			retVal.InjectFrom(promotion);

			if (promotion.DynamicExpression != null && promotion.DynamicExpression.Children != null)
			{
				var conditionExpression = promotion.DynamicExpression.GetConditionExpression();
				retVal.PredicateSerialized = SerializationUtil.SerializeExpression(conditionExpression);
				var rewards = promotion.DynamicExpression.GetRewards();
				retVal.RewardsSerialized = JsonConvert.SerializeObject(rewards, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
			
				//Clear availableElements in expression (for decrease size)
				var allBlocks =	((DynamicBlockExpression)promotion.DynamicExpression).Traverse(x => x.Children != null ? x.Children.OfType<DynamicBlockExpression>() : null);
				foreach(var block in allBlocks)
				{
					block.AvailableChildren = null;
				}
				retVal.PredicateVisualTreeSerialized = JsonConvert.SerializeObject(promotion.DynamicExpression);

			}
			return retVal;
		}
	}
}

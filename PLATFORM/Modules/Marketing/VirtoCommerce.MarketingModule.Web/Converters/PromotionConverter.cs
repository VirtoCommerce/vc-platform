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
using VirtoCommerce.MarketingModule.Data.Common;
using VirtoCommerce.MarketingModule.Data.Promotions;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.DynamicExpressionModule.Data.Promotion;
using VirtoCommerce.DynamicExpressionModule.Data;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class PromotionConverter
	{
		public static webModel.Promotion ToWebModel(this coreModel.Promotion promotion, PromoDynamicExpressionTree etalonEpressionTree = null)
		{
			var retVal = new webModel.Promotion();
			retVal.InjectFrom(promotion);
			retVal.Coupons = promotion.Coupons;
			retVal.Type = promotion.GetType().Name;
			var dynamicPromotion = promotion as DynamicPromotion;
			if (dynamicPromotion != null && etalonEpressionTree != null)
			{
				retVal.DynamicExpression = etalonEpressionTree;
				if (!String.IsNullOrEmpty(dynamicPromotion.PredicateVisualTreeSerialized))
				{
					retVal.DynamicExpression = JsonConvert.DeserializeObject<PromoDynamicExpressionTree>(dynamicPromotion.PredicateVisualTreeSerialized);
					//Copy available elements from etalon because they not persisted
					var sourceBlocks = ((DynamicExpression)etalonEpressionTree).Traverse(x => x.AvailableChildren);
					var targetBlocks = ((DynamicExpression)retVal.DynamicExpression).Traverse(x => x.Children);
					foreach (var sourceBlock in sourceBlocks)
					{
						foreach(var targetBlock in  targetBlocks.Where(x => x.Id == sourceBlock.Id))
						{
							targetBlock.AvailableChildren = sourceBlock.AvailableChildren;
						}
					}
					//copy available elements from etalon
					retVal.DynamicExpression.AvailableChildren = etalonEpressionTree.AvailableChildren;
				}
			}
			return retVal;
		}

		public static coreModel.Promotion ToCoreModel(this webModel.Promotion promotion)
		{
			var retVal = new DynamicPromotion();
			retVal.InjectFrom(promotion);
			retVal.Coupons = promotion.Coupons;

			if (promotion.DynamicExpression != null && promotion.DynamicExpression.Children != null)
			{
				var conditionExpression = promotion.DynamicExpression.GetConditionExpression();
				retVal.PredicateSerialized = SerializationUtil.SerializeExpression(conditionExpression);
				var rewards = promotion.DynamicExpression.GetRewards();
				retVal.RewardsSerialized = JsonConvert.SerializeObject(rewards, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
			
				//Clear availableElements in expression (for decrease size)
				promotion.DynamicExpression.AvailableChildren = null;
				var allBlocks = ((DynamicExpression)promotion.DynamicExpression).Traverse(x => x.Children);
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

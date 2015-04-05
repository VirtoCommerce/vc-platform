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
					retVal.AddRange(TryConvert2LineItemRewards(promoContext, reward));
				}
			}

			return retVal.ToArray();
		}

		public override PromotionReward[] ProcessEvent(MarketingEvent marketingEvent)
		{
			return null;
		}

		private PromotionReward[] TryConvert2LineItemRewards(PromotionEvaluationContext context, PromotionReward reward)
		{
			var retVal = new PromotionReward[] { reward };
			//Is Shopping cart context
			if (context.Product == null)
			{
				var catalogItemReward = reward as CatalogItemAmountReward;
				var cartSubtotalReward = reward as CartSubtotalReward;
				if (catalogItemReward != null)
				{
					//Convert CatalogItem reward to lineItem rewards
					retVal = TryConvert2LineItemRewards(context, catalogItemReward);
				}
				else if (cartSubtotalReward != null)
				{
					//Convert cart subtotal relative reward to line item relative rewards
					retVal = TryConvert2LineItemRewards(context, cartSubtotalReward);
				}
			}
			return retVal;
		}

		private PromotionReward[] TryConvert2LineItemRewards(PromotionEvaluationContext context, CatalogItemAmountReward reward)
		{
			var retVal = new List<PromotionReward>();
			foreach (var lineItem in context.ShoppingCart.Items)
			{
				if (!(lineItem.DiscountTotal > 0) && lineItem.ProductId == reward.ProductId)
				{
					var newReward = new LineItemAmountReward()
					{
						Promotion = this,
						Amount = reward.Amount,
						AmountType = reward.AmountType,
						LineItemId = lineItem.Id,
						IsValid = reward.IsValid
					};
					retVal.Add(newReward);
				}
			}
			return retVal.ToArray();
		}

		private PromotionReward[] TryConvert2LineItemRewards(PromotionEvaluationContext context, CartSubtotalReward reward)
		{
			var retVal = new List<PromotionReward>();
			foreach (var lineItem in context.ShoppingCart.Items)
			{
				if (!(lineItem.DiscountTotal > 0))
				{
					var newReward = new LineItemAmountReward()
					{
						Promotion = this,
						Amount = reward.Amount,
						AmountType = RewardAmountType.Relative,
						LineItemId = lineItem.Id,
						IsValid = reward.IsValid
					};
					retVal.Add(newReward);
				}
			}
			return retVal.ToArray();
		}
		
	}
}

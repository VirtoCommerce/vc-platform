using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class DefaultMarketingPromoEvaluatorImpl : IMarketingPromoEvaluator
	{
		private readonly IMarketingService _marketingService;
		public DefaultMarketingPromoEvaluatorImpl(IMarketingService marketingService)
		{
			_marketingService = marketingService;
		}

		#region IMarketingEvaluator Members
		public PromotionResult EvaluatePromotion(IPromotionEvaluationContext context)
		{
			var now = DateTime.UtcNow;
			var promotions = _marketingService.GetActivePromotions();

			var promoContext = (PromotionEvaluationContext)context;

			var retVal = new PromotionResult();

			var rewards = promotions.SelectMany(x => x.EvaluatePromotion(context)).ToArray();

			//best shipment promotion
			var curShipmentAmount = promoContext.Shipment != null ? promoContext.Shipment.Price : 0m;
			var allShipmentRewards = rewards.OfType<ShipmentReward>().ToArray();
			EvaluteBestAmountRewards(curShipmentAmount, allShipmentRewards).ToList().ForEach(x => retVal.Rewards.Add(x));

			//best item promotion
			if (promoContext.Product != null)
			{
				var allCatalogItemRewards = rewards.OfType<CatalogItemAmountReward>().ToArray();
				EvaluteBestAmountRewards(promoContext.ProductPrice ?? 0, allCatalogItemRewards).ToList().ForEach(x => retVal.Rewards.Add(x));
			}
			else
			{
				var allLineItemRewards = rewards.OfType<LineItemAmountReward>().ToArray();
				var groupRewards = allLineItemRewards.GroupBy(x => x.LineItemId);
				foreach (var groupReward in groupRewards)
				{
					var lineItem = promoContext.ShoppingCart.Items.First(x => x.Id == groupReward.Key);
					EvaluteBestAmountRewards(lineItem.PlacedPrice, groupReward.ToArray()).ToList().ForEach(x => retVal.Rewards.Add(x));
				}
			}

			//best order promotion 
			var cartSubtotalRewards = rewards.OfType<CartSubtotalReward>().Where(x => x.IsValid).OrderByDescending(x => x.Amount);
			var cartSubtotalReward = cartSubtotalRewards.FirstOrDefault(x => !string.IsNullOrEmpty(x.Coupon)) ?? cartSubtotalRewards.FirstOrDefault();
			if (cartSubtotalReward != null)
			{
				retVal.Rewards.Add(cartSubtotalReward);

				//Exlusive offer
				if (cartSubtotalReward.IsExclusive)
				{
					var lineItemRewards = retVal.Rewards.OfType<LineItemAmountReward>().ToList();
					for (var i = lineItemRewards.Count - 1; i >= 0; i--)
					{
						retVal.Rewards.Remove(lineItemRewards[i]);
					}
				}
			}
			var potentialCartSubtotalRewards = rewards.OfType<CartSubtotalReward>().Where(x => !x.IsValid).OrderByDescending(x => x.Amount);
			var potentialCartSubtotalReward = potentialCartSubtotalRewards.FirstOrDefault(x => !string.IsNullOrEmpty(x.Coupon)) ?? cartSubtotalRewards.FirstOrDefault();
			if (potentialCartSubtotalReward != null && cartSubtotalReward != null)
			{
				if (potentialCartSubtotalReward.Amount > cartSubtotalReward.Amount)
				{
					retVal.Rewards.Add(potentialCartSubtotalReward);
				}
			}
			else if (potentialCartSubtotalReward != null)
			{
				retVal.Rewards.Add(potentialCartSubtotalReward);
			}


			//Gifts
			rewards.OfType<GiftCatalogItemReward>().ToList().ForEach(x => retVal.Rewards.Add(x));

			//Special offer
			rewards.OfType<SpecialOfferReward>().ToList().ForEach(x => retVal.Rewards.Add(x));

			return retVal;
		}

		public CatalogPromotionResult[] EvaluateCatalogPromotions(IPromotionEvaluationContext context)
		{
			var retVal = new List<CatalogPromotionResult>();

			var now = DateTime.UtcNow;
			var promotions = _marketingService.GetActivePromotions();
			retVal.AddRange(promotions.Select(x => x.EvaluateCatalogPromotion(context)).Where(x => x != null));

			return retVal.ToArray();
		}

		public PromotionResult ProcessEvent(MarketingEvent markertingEvent)
		{
			var retVal = new PromotionResult();
			var promotions = _marketingService.GetActivePromotions();
			foreach (var promotion in promotions)
			{
				var rewards = promotion.ProcessEvent(markertingEvent).Where(x => x != null);
				foreach (var promotionReward in rewards)
				{
					retVal.Rewards.Add(promotionReward);
				}
			}

			return retVal;
		}

		#endregion

		private static AmountBasedReward[] EvaluteBestAmountRewards(decimal currentAmount, AmountBasedReward[] promoRewards)
		{
			var retVal = new List<AmountBasedReward>();

			var bestReward = EvaluateBestAmountReward(currentAmount, promoRewards.Where(x => x.IsValid).ToArray());
			if (bestReward != null)
			{
				retVal.Add(bestReward);
			}

			var bestPotentialReward = EvaluateBestAmountReward(currentAmount, promoRewards.Where(x => !x.IsValid).ToArray());

			if (bestPotentialReward != null && bestReward != null)
			{
				var bestRewardFromTwo = bestReward = EvaluateBestAmountReward(currentAmount, new AmountBasedReward[] { bestReward, bestPotentialReward });
				if (bestRewardFromTwo == bestPotentialReward)
				{
					retVal.Add(bestPotentialReward);
				}
			}
			else if (bestPotentialReward != null)
			{
				retVal.Add(bestPotentialReward);
			}

			return retVal.ToArray();
		}

		private static AmountBasedReward EvaluateBestAmountReward(decimal currentAmount, AmountBasedReward[] promoRewards)
		{
			AmountBasedReward retVal = null;
			var maxAbsoluteReward = promoRewards
				.Where(y => y.AmountType == RewardAmountType.Absolute)
				.OrderByDescending(y => y.Amount).FirstOrDefault();

			var maxRelativeReward = promoRewards
				.Where(y => y.AmountType == RewardAmountType.Relative)
				.OrderByDescending(y => y.Amount).FirstOrDefault();


			var absDiscountAmount = maxAbsoluteReward != null ? maxAbsoluteReward.Amount : 0;
			var relDiscountAmount = maxRelativeReward != null ? currentAmount * maxRelativeReward.Amount : 0;

			if (maxAbsoluteReward != null && maxRelativeReward != null)
			{
				if (absDiscountAmount > relDiscountAmount)
				{
					retVal = maxAbsoluteReward;
				}
				else
				{
					retVal = maxRelativeReward;
				}
			}
			else if (maxAbsoluteReward != null)
			{
				retVal = maxAbsoluteReward;
			}
			else if (maxRelativeReward != null)
			{
				retVal = maxRelativeReward;
			}

			return retVal;
		}

	
	}
}

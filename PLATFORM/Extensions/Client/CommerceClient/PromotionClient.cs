using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.Client
{
	public class PromotionClient
	{
		#region Cache Constants

		#endregion

		#region Private Variables

		private readonly ICacheRepository _cacheRepository;
		private readonly ICustomerSessionService _customerSession;
	    private readonly IPromotionEvaluator _evaluator;
	    private readonly IMarketingRepository _marketingRepository;

		#endregion

		private CacheHelper _cacheHelper;

	    /// <summary>
	    ///     Initializes the <see cref="PromotionClient" /> class.
	    /// </summary>
	    /// <param name="marketingRepository">The marketing repository.</param>
	    /// <param name="customerSession">The customer session.</param>
	    /// <param name="evaluator"></param>
	    /// <param name="cacheRepository">The cache repository.</param>
	    public PromotionClient(IMarketingRepository marketingRepository, 
            ICustomerSessionService customerSession,
            IPromotionEvaluator evaluator, 
            ICacheRepository cacheRepository)
		{
			_marketingRepository = marketingRepository;
			_cacheRepository = cacheRepository;
			_customerSession = customerSession;
	        _evaluator = evaluator;
		}

		public CacheHelper Helper
		{
			get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
		}

		public decimal GetItemDiscountPrice(Item item, Price lowestPrice, Hashtable tags = null)
		{
			var price = lowestPrice.Sale ?? lowestPrice.List;

			var session = _customerSession.CustomerSession;

			// apply discounts
			// create context 
			var ctx = new Dictionary<string, object>();
			var set = GetPromotionEntrySetFromItem(item, price, tags);
			ctx.Add(PromotionEvaluationContext.TargetSet, set);

			var evaluationContext = new PromotionEvaluationContext
				{
					ContextObject = ctx,
					CustomerId = session.CustomerId,
					CouponCode = null,
					Currency = session.Currency,
					PromotionType = PromotionType.CatalogPromotion,
					Store = session.StoreId,
					IsRegisteredUser = session.IsRegistered,
					IsFirstTimeBuyer = session.IsFirstTimeBuyer
				};

			var promotions = _evaluator.EvaluatePromotion(evaluationContext);
            var rewards = promotions.SelectMany(x => x.Rewards).ToArray();
            var records = new List<PromotionRecord>();

            records.AddRange(rewards.Select(reward => new PromotionRecord
            {
                AffectedEntriesSet = set,
                TargetEntriesSet = set,
                Reward = reward,
                PromotionType = PromotionType.CatalogPromotion
            }));

            //Filter by policies
		    var allRecords = _evaluator.EvaluatePolicies(records.ToArray());

            var lineItemRewards = allRecords.Select(x=>x.Reward).OfType<CatalogItemReward>().ToArray();

			var discountTotal = 0m;
			foreach (var reward in lineItemRewards)
			{
				if (reward.QuantityLimit > 1) // skip everything for higher quantity
				{
					continue;
				}

				if (!String.IsNullOrEmpty(reward.SkuId)) // filter out free item rewards
				{
					if (!item.ItemId.Equals(reward.SkuId, StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
				}

				var discountAmount = 0m;
				const int quantity = 1;
				if (reward.AmountTypeId == (int)RewardAmountType.Relative)
				{
					discountAmount = Math.Round(quantity*price*reward.Amount*0.01m,2);
				}
				else if (reward.AmountTypeId == (int)RewardAmountType.Absolute)
				{
					discountAmount =  Math.Round(quantity*reward.Amount,2);
				}
				discountTotal += discountAmount;
			}

			return discountTotal;
		}

		public bool IsSkuItemPromotion(string skuId, out string promotionId)
		{
			var reward = _marketingRepository.PromotionRewards.OfType<CatalogItemReward>().FirstOrDefault(pr => pr.SkuId == skuId);
			promotionId = reward != null ? reward.PromotionId : null;
			return reward != null;
		}

		private PromotionEntrySet GetPromotionEntrySetFromItem(Item item, decimal price, Hashtable tags)
		{
			var set = new PromotionEntrySet();
			var populate = new PromotionEntryPopulate();

			var entry = new PromotionEntry(tags ?? new Hashtable()) { CostPerEntry = price };
			populate.Populate(ref entry, item);
			set.Entries.Add(entry);

			return set;
		}
	}
}
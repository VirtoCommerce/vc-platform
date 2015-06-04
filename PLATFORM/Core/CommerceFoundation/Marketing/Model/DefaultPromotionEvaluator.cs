using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;


namespace VirtoCommerce.Foundation.Marketing.Model
{
	public sealed class DefaultPromotionEvaluator : EvaluatorBase, IPromotionEvaluator
    {
        #region Private Variables
        private readonly IMarketingRepository _repository;
		private readonly IEvaluationPolicy[] _policies;
		private readonly IPromotionUsageProvider _usageProvider;
        
        private static bool _isEnabled;
        #endregion

        public const string PromotionsCacheKey = "M:P:{0}";

	    public DefaultPromotionEvaluator(IMarketingRepository repository, IPromotionUsageProvider usageProvider, IEvaluationPolicy[] policies, ICacheRepository cache)
			:base(cache)
		{
			_repository = repository;
			_usageProvider = usageProvider;
			_policies = policies;

            _isEnabled = MarketingConfiguration.Instance.Cache.IsEnabled;
            Cache = new CacheHelper(cache);
		}

		#region IPromotionEvaluator Members

		public Promotion[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
            if (!(context.ContextObject is Dictionary<string, object>))
                throw new ArgumentException("context.ContextObject must be a Dictionary");

			var query = GetPromotions();
			var utcnow = DateTime.UtcNow;

			switch (context.PromotionType)
			{
				case PromotionType.CatalogPromotion:
					query = query.OfType<CatalogPromotion>();
					break;
				case PromotionType.ShipmentPromotion:
				case PromotionType.CartPromotion:
					query = query.OfType<CartPromotion>();
					if (!string.IsNullOrEmpty(context.Store))
					{
						query = query.Cast<CartPromotion>().Where(x => x.StoreId == context.Store);

					}
					break;
			}

            // sort promotions by type and priority
            query = query.OrderByDescending(x => x.GetType().Name).ThenByDescending(x => x.Priority);

			//filter by date expiration
            query = query.Where(x => x.Status.Equals(PromotionStatus.Active.ToString()) && (x.StartDate == null || utcnow >= x.StartDate) && (x.EndDate == null || x.EndDate >= utcnow));

			//Evaluate query
			var promotions = query.ToArray();
			Func<Promotion, bool> isValidCouponPredicate = x => false;
			//filter by entered coupons 
            if (!string.IsNullOrEmpty(context.CouponCode))
            {
                //check coupon code if specified coupon set for promotion
				isValidCouponPredicate = x =>
                    {
                        var coupons = new string[] { };
                        if (x.CouponId != null)
                        {
                            coupons = _repository.Coupons.Where(c => c.CouponId == x.CouponId).Select(c => c.Code).ToArray();
                        }
                        else if (x.CouponSetId != null)
                        {
                            coupons = _repository.CouponSets.Expand(c => c.Coupons)
                                                 .Where(c => c.CouponSetId == x.CouponSetId)
                                                 .SelectMany(c => c.Coupons)
                                                 .Select(c => c.Code).ToArray();
                        }

                        var retVal = coupons.Contains(context.CouponCode);

                        return retVal;
                    };
            }

			//Allow only those rewards that do not require coupons or check if coupon is valid
			promotions = promotions.Where(x => isValidCouponPredicate(x) || string.IsNullOrEmpty(x.CouponId) && string.IsNullOrEmpty(x.CouponSetId)).ToArray();
		
			//Filter promotion by usage limit
			promotions = promotions.Where(x => x.TotalLimit <= 0 || _usageProvider.GetTotalUsageCount(x.PromotionId) < x.TotalLimit).ToArray();
			promotions = promotions.Where(x => x.PerCustomerLimit <= 0 || _usageProvider.GetUsagePerCustomerCount(x.PromotionId, context.CustomerId) < x.PerCustomerLimit).ToArray();
			
			//TODO: Filter by customer segments
						
			//Evaluate promotions
			Func<Promotion, bool> conditionPredicate = x =>
				{
					var condition = DeserializeExpression<Func<IEvaluationContext, bool>>(x.PredicateSerialized);
					return condition(context);
				};
			
			promotions = promotions.Where(conditionPredicate).ToArray();
			return promotions;
		}

	    public PromotionRecord[] EvaluatePolicies(PromotionRecord[] records, IEvaluationPolicy[] policies = null)
	    {
	        policies = policies ?? _policies;
			records = SortPromotionRecords(records);
			return policies.Aggregate(records, (current, policy) => policy.FilterPromotions(null, current));
	    }

	    #endregion
		
        private IQueryable<Promotion> GetPromotions()
        {
            return Cache.Get(
                CacheHelper.CreateCacheKey(Constants.PromotionsCachePrefix,  string.Format(PromotionsCacheKey, "all")),
                () => (_repository.Promotions.ExpandAll()
                    .Expand(p=>p.Coupon) //ExpandAll skips non collection props
                    .Expand(p=>p.CouponSet)).ToArray(),
                MarketingConfiguration.Instance.Cache.PromotionsTimeout,
                _isEnabled).AsQueryable();
        }


        // make sure to sort the records correctly
        // 1st: items with a coupon applied
        // 2nd: catalog items
        // 3rd: order
        // 4th: shipping
        private PromotionRecord[] SortPromotionRecords(PromotionRecord[] records)
        {
            var all = new List<PromotionRecord>();
            var recordsWithCoupons = from r in records where !String.IsNullOrEmpty(r.Reward.Promotion.CouponId) orderby r.Reward.Promotion.Priority descending select r;

            // all all coupon records first
            all.Add(recordsWithCoupons);

            var catalogRecords = from r in records where r.PromotionType == PromotionType.CatalogPromotion && !all.Contains(r) orderby r.Reward.Promotion.Priority descending select r;
            all.Add(catalogRecords);

            var cartRecords = from r in records where r.PromotionType == PromotionType.CartPromotion && !all.Contains(r) orderby r.Reward.Promotion.Priority descending select r;
            all.Add(cartRecords);

            return all.ToArray();
        }
	
	}
}

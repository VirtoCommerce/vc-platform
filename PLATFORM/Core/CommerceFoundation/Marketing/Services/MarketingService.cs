using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Foundation.Marketing.Services
{
	public class MarketingService : IMarketingService
	{
		private IMarketingRepository _marketingRepository;
		private IPromotionEvaluator _evaluator;

		public MarketingService(IMarketingRepository marketingRepository,
								IPromotionEvaluator evaluator)
		{
			_marketingRepository = marketingRepository;
			_evaluator = evaluator;
		}

		#region IMarketingService Members

		public Model.Promotion[] EvaluatePromotions(IPromotionEvaluationContext context)
		{
			return _evaluator.EvaluatePromotion(context);
		}

		public void RegisterToUsePromotion(IPromotionEvaluationContext context, Promotion promotion)
		{
			var promotionUsage = new PromotionUsage
			{
				CouponCode = context.CouponCode,
				MemberId = context.CustomerId,
				PromotionId = promotion.PromotionId,
				OrderGroupId = ((OrderGroup)context.ContextObject).OrderGroupId
			};

            _marketingRepository.Add(promotionUsage);

            _marketingRepository.UnitOfWork.Commit();
		}

		#endregion
	}
}

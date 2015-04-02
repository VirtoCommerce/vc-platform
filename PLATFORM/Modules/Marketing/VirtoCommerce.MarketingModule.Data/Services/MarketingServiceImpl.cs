using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.MarketingModule.Data.Repositories;
using foundationModel = VirtoCommerce.Foundation.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.CustomerModule.Data.Converters;
using ExpressionSerialization;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class MarketingServiceImpl : ServiceBase, IMarketingService
    {
		private readonly Func<IFoundationMarketingRepository> _repositoryFactory;
		private readonly IPromotionExtensionManager _customPromotionManager;

		public MarketingServiceImpl(Func<IFoundationMarketingRepository> repositoryFactory, IPromotionExtensionManager customPromotionManager)
        {
			_repositoryFactory = repositoryFactory;
			_customPromotionManager = customPromotionManager;
        }

        #region IMarketingService Members

		public Promotion[] GetActivePromotions()
		{
			var retVal = new List<Promotion>(_customPromotionManager.Promotions);
			using (var repository = _repositoryFactory())
			{
				var dbStoredPromotions = repository.GetActivePromotions().Select(x => x.ToCoreModel()).ToList();
				var promoComparer = AnonymousComparer.Create((coreModel.Promotion x) => x.Id);
				dbStoredPromotions.Patch(retVal, promoComparer, (source, target) => target.InjectFrom(source));
			}
			return retVal.OrderBy(x => x.Priority).ToArray();
		}

		public Promotion GetPromotionById(string id)
		{
			coreModel.Promotion retVal = null;
			using (var repository = _repositoryFactory())
			{
				var entity = repository.GetPromotionById(id);
		
				if (entity != null)
				{
					retVal = entity.ToCoreModel();
				}
			}

			if(retVal == null)
			{
				retVal = _customPromotionManager.Promotions.FirstOrDefault(x => x.Id == id);
			}
			return retVal;
		}

		public Promotion CreatePromotion(Promotion promotion)
		{
			var entity = promotion.ToFoundation();
			coreModel.Promotion retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
			}
			retVal = GetPromotionById(entity.PromotionId);
			return retVal;
		}

		public void UpdatePromotions(Promotion[] promotions)
		{
			using (var repository = _repositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var promotion in promotions)
				{
					var sourceEntity = promotion.ToFoundation();
					var targetEntity = repository.GetPromotionById(promotion.Id);
					if (targetEntity == null)
					{
						repository.Add(sourceEntity);
					}
					else
					{
						changeTracker.Attach(targetEntity);
						sourceEntity.Patch(targetEntity);
					}
				}
				CommitChanges(repository);
			}
		}

		public void DeletePromotions(string[] ids)
		{
			throw new NotImplementedException();
		}

		public Coupon GetCouponById(string id)
		{
			throw new NotImplementedException();
		}

		public Coupon[] GetPersonalCoupons(string customerId)
		{
			throw new NotImplementedException();
		}

		public Promotion CreateCoupon(Coupon coupon)
		{
			throw new NotImplementedException();
		}

		public void UpdateCoupons(Coupon[] coupons)
		{
			throw new NotImplementedException();
		}

		public void DeleteCoupons(string[] ids)
		{
			throw new NotImplementedException();
		}
 
        #endregion
	}
}

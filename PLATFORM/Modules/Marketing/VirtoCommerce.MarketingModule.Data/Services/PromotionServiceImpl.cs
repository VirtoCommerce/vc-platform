using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using foundationModel = VirtoCommerce.MarketingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using Omu.ValueInjecter;
using VirtoCommerce.CustomerModule.Data.Converters;
using ExpressionSerialization;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Services
{
	public class PromotionServiceImpl : ServiceBase, IPromotionService
    {
		private readonly Func<IMarketingRepository> _repositoryFactory;
		private readonly IMarketingExtensionManager _customPromotionManager;

		public PromotionServiceImpl(Func<IMarketingRepository> repositoryFactory, IMarketingExtensionManager customPromotionManager)
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
			retVal = GetPromotionById(entity.Id);
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
			using (var repository = _repositoryFactory())
			{
				foreach (var id in ids)
				{
					var entity = repository.GetPromotionById(id);
					repository.Remove(entity);
				}
				CommitChanges(repository);
			}
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

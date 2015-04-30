using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Promotions;
using ExpressionSerialization;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class PromotionConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Promotion ToCoreModel(this dataModel.Promotion dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new DynamicPromotion();
			retVal.InjectFrom(dbEntity);
			retVal.Coupons = dbEntity.Coupons.Select(x=>x.Code).ToArray();
			retVal.Store = dbEntity.StoreId;
			retVal.MaxUsageCount = dbEntity.TotalLimit;
			retVal.MaxPersonalUsageCount = dbEntity.PerCustomerLimit;

			return retVal;
		}


		public static dataModel.Promotion ToDataModel(this coreModel.Promotion promotion)
		{
			if (promotion == null)
				throw new ArgumentNullException("promotion");

			var retVal = new dataModel.Promotion();
			retVal.StartDate = DateTime.UtcNow;
			retVal.InjectFrom(promotion);
	
			retVal.StartDate = promotion.StartDate ?? DateTime.UtcNow;
			retVal.StoreId = promotion.Store;
			if (promotion.Coupons != null)
			{
				retVal.Coupons = new ObservableCollection<dataModel.Coupon>(promotion.Coupons.Select(x => new dataModel.Coupon { Code = x }));
			}
			retVal.TotalLimit = promotion.MaxUsageCount;
			retVal.PerCustomerLimit = promotion.MaxPersonalUsageCount;
		
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.Promotion source, dataModel.Promotion target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<dataModel.Promotion>(x => x.Name, x => x.Description, x=>x.Priority, x=>x.CouponCode, x=>x.StoreId,
																		   x => x.StartDate, x => x.EndDate, x => x.IsActive, x => x.TotalLimit, x => x.PerCustomerLimit, x => x.PredicateSerialized,
																		   x => x.PredicateVisualTreeSerialized, x => x.RewardsSerialized);
		
			target.InjectFrom(patchInjection, source);

			if (!source.Coupons.IsNullCollection())
			{
				var couponComparer = AnonymousComparer.Create((dataModel.Coupon x) => x.Code);
				source.Coupons.Patch(target.Coupons, (sourceCoupon, targetCoupon) => { return; });
			}
		}


	}
}

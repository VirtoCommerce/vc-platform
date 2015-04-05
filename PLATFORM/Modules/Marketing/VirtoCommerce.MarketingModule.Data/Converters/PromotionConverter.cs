using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundationModel = VirtoCommerce.Foundation.Marketing.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data.Promotions;
using ExpressionSerialization;

namespace VirtoCommerce.CustomerModule.Data.Converters
{
	public static class PromotionConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Promotion ToCoreModel(this foundationModel.Promotion dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new DynamicPromotion();
			retVal.InjectFrom(dbEntity);
			retVal.Id = dbEntity.PromotionId;

			retVal.TotalUsageLimit = dbEntity.TotalLimit;
			retVal.PerCustomerUsageLimit = dbEntity.PerCustomerLimit;

			retVal.CreatedDate = dbEntity.Created.Value;
			retVal.ModifiedDate = dbEntity.LastModified;

			var status = (foundationModel.PromotionStatus)Enum.Parse(typeof(foundationModel.PromotionStatus), dbEntity.Status);
			retVal.IsActive = status == foundationModel.PromotionStatus.Active;
		
			return retVal;
		}


		public static foundationModel.Promotion ToFoundation(this coreModel.Promotion promotion)
		{
			if (promotion == null)
				throw new ArgumentNullException("promotion");

			//TODO: Need rid for typized promotion in db. Temporary  save all promotion as catalogPromotion
			var retVal = new foundationModel.CatalogPromotion();
			retVal.StartDate = DateTime.UtcNow;
			retVal.InjectFrom(promotion);

			if (promotion.Id != null)
			{
				retVal.PromotionId = promotion.Id;
			}
			retVal.TotalLimit = promotion.TotalUsageLimit;
			retVal.PerCustomerLimit = promotion.PerCustomerUsageLimit;
			retVal.Status = promotion.IsActive ? foundationModel.PromotionStatus.Active.ToString() : foundationModel.PromotionStatus.Inactive.ToString();
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Promotion source, foundationModel.Promotion target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<foundationModel.Promotion>(x => x.Name, x => x.Description,
																		   x => x.StartDate, x => x.EndDate,
																		   x => x.Status, x => x.TotalLimit, x => x.PerCustomerLimit, x => x.PredicateSerialized,
																		   x => x.PredicateVisualTreeSerialized, x => x.RewardsSerialized);
			target.InjectFrom(patchInjection, source);
		}


	}
}

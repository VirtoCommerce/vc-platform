using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class Promotion : Entity, IAuditable
	{
		public Promotion()
		{
			IsActive = true;
			Id = Name = this.GetType().Name;
		}

		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string Store { get; set; }
		public string Name { get; set; }
	
		public bool IsActive { get; set; }

		public int Priority { get; set; }

		public string Coupon { get; set; }

		public string Description { get; set; }

		public int TotalUsageLimit { get; set; }
		public int PerCustomerUsageLimit { get; set; }
		
		public DateTime? StartDate { get; set; }
		
		public DateTime? EndDate { get; set; }

		public ICollection<PromotionUsage> Usages { get; set; }

		public virtual PromotionReward[] EvaluatePromotion(IPromotionEvaluationContext context)
		{
			return new PromotionReward[] { };
		}

		public virtual CatalogPromotionResult EvaluateCatalogPromotion(IPromotionEvaluationContext context)
		{
			return null;
		}

		public virtual PromotionReward[] ProcessEvent(IMarketingEvent marketingEvent)
		{
			return new PromotionReward[] { };
		}
	}
}

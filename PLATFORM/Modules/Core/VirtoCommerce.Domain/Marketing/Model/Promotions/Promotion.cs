using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class Promotion : AuditableEntity
	{
		public Promotion()
		{
			IsActive = true;
			Id = Name = this.GetType().Name;
		}

		public string Store { get; set; }
		public string Name { get; set; }
	
		public bool IsActive { get; set; }

		public int Priority { get; set; }

		public string[] Coupons { get; set; }

		public string Description { get; set; }

		public int MaxUsageCount { get; set; }
		public int MaxPersonalUsageCount { get; set; }
		
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface IMarketingPromoEvaluator
	{
		/// <summary>
		/// Evaluate promotion for specific context
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		PromotionResult EvaluatePromotion(IPromotionEvaluationContext context);

		/// <summary>
		/// Return list element for promotion catalog
		/// </summary>
		/// <param name="publicationPlaces"></param>
		/// <returns></returns>
		CatalogPromotionResult[] EvaluateCatalogPromotions(IPromotionEvaluationContext context);

		/// <summary>
		/// External event handler
		/// </summary>
		PromotionResult ProcessEvent(IMarketingEvent marketingEvent);
	}
}

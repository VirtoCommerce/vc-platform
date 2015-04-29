using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public abstract class CatalogPromotionResult
	{
		public CatalogPromotionResult(Promotion promo)
		{
			CatalogPromotionId = promo.Name;
			Description = promo.Description;
		}
		public bool IsValid { get; set; }

		public string CatalogPromotionId { get; set; }
		public string Description { get; set; }
	}
}

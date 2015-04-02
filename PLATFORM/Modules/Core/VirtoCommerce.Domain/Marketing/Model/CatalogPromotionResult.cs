using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	/// <summary>
	/// Элемент каталога МП
	/// </summary>
	public abstract class CatalogPromotionResult
	{
		public CatalogPromotionResult(Promotion promo)
		{
			CatalogPromotionId = promo.Name;
			Description = promo.Description;
		}
		/// <summary>
		/// Признак действия
		/// </summary>
		public bool IsValid { get; set; }

		public string CatalogPromotionId { get; set; }
		public string Description { get; set; }
	}
}

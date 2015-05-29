using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public class DefaultMarketingExtensionManagerImpl : IMarketingExtensionManager
	{
		private List<Promotion> _promotions = new List<Promotion>();

		#region InMemoryExtensionManagerImpl Members

		public PromoDynamicExpressionTree PromotionDynamicExpressionTree { get; set; }
		public ConditionExpressionTree DynamicContentExpressionTree { get; set; }

		public void RegisterPromotion(Promotion promotion)
		{
			if (promotion == null)
			{
				throw new ArgumentNullException("promotion");
			}
			if (_promotions.Any(x => x.Id == promotion.Id))
			{
				throw new OperationCanceledException(promotion.Id + " already registered");
			}
			_promotions.Add(promotion);
		}

		public IEnumerable<Promotion> Promotions
		{
			get { return _promotions.AsReadOnly(); }
		}

		#endregion
	}
}

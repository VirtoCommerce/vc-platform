using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface ICustomPromotionManager
	{
		CompositeElement DynamicExpression { get; set; }
		void RegisterPromotion(Promotion promotion);
		IEnumerable<Promotion> Promotions { get; }
	}
}

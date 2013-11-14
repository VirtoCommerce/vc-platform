using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public interface IPromotionEvaluator
	{
		Promotion[] EvaluatePromotion(IPromotionEvaluationContext context);
	}
}

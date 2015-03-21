using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public interface IPromotionEvaluationContext: IEvaluationContext
	{
		string CouponCode { get; }
		string CustomerId { get; }
        string PromotionType { get; }
		string Store { get; }
	}
}

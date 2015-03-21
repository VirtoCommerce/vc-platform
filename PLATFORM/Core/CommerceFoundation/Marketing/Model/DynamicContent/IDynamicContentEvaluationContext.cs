using System;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	public interface IDynamicContentEvaluationContext: IEvaluationContext
	{
		string CustomerId { get; set; }
		string ContentPlace { get; set; }
		DateTime CurrentDate { get; set; }
	}
}

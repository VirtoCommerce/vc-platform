using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface IMarketingDynamicContentEvaluator
	{
		DynamicContentItem[] EvaluateItems(IDynamicContentEvaluationContext context);
	}
}

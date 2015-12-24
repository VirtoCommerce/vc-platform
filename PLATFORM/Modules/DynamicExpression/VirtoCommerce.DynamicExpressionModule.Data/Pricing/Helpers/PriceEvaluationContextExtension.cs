using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Pricing.Model;

namespace VirtoCommerce.DynamicExpressionModule.Data.Pricing
{
	public static class PricingEvaluationContextExtension
	{
		#region Dynamic expression evaluation helper methods
		public static bool TagsContains(this PriceEvaluationContext context, string tag)
		{
            var retVal = context.Tags != null;
            if (retVal)
            {
                retVal = context.Tags.Any(x => String.Equals(x, tag, StringComparison.InvariantCultureIgnoreCase));
            }
			return retVal;
		}
	
		#endregion

	}
}

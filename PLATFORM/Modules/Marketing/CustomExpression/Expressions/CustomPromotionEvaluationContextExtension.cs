using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;

namespace CustomExpression
{
	public static class CustomPromotionEvaluationContextExtension
	{

		#region Dynamic expression evaluation helper methods
		public static bool CheckItemTags(this PromotionEvaluationContext context, string[] tags)
		{
			var retVal = tags.Any(x=> context.PromoEntry.Attributes.ContainsKey("tag") && String.Equals(context.PromoEntry.Attributes["tag"], x, StringComparison.InvariantCultureIgnoreCase));
			return retVal;
		}
		#endregion
	}
}

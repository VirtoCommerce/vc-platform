using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;
using VirtoCommerce.MarketingModule.Expressions;
using VirtoCommerce.MarketingModule.Expressions.Promotion;

namespace VirtoCommerce.MarketingModule.Test.CustomDynamicPromotionExpressions
{
	//items with [] tag
	public class ConditionItemWithTag : DynamicExpression, IConditionExpression
	{
		public string[] Tags { get; set; }

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).CheckItemTags() > NumItem
		/// </summary>
		/// <returns></returns>
		linq.Expression<Func<IPromotionEvaluationContext, bool>> IConditionExpression.GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.Convert(paramX, typeof(PromotionEvaluationContext));

			var tagsArray = linq.Expression.NewArrayInit(typeof(string), Tags.Select(x=> linq.Expression.Constant(x)));

			var methodInfo = typeof(CustomPromotionEvaluationContextExtension).GetMethod("CheckItemTags");
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, tagsArray);
			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);
			return retVal;
		}

		#endregion
	}
}

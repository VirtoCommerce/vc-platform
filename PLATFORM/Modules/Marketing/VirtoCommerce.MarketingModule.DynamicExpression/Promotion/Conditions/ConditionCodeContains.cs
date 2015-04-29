using System;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Product code contains []
	public class ConditionCodeContains : ConditionBase, IConditionExpression
	{
		public string Keyword { get; set; }

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).IsItemCodeContains(Keyword)
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("IsItemCodeContains");

			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(Keyword));

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}

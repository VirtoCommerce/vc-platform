using System;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.DynamicExpression.Promotion
{
	//Cart subtotal is []
	public class ConditionCartSubtotalLeast : ConditionBase, IConditionExpression
	{
		public decimal SubTotal { get; set; }
		public bool Exactly { get; set; }

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).GetCartTotalWithExcludings(ExcludingCategoryIds, ExcludingProductIds) > SubTotal
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var subTotal = linq.Expression.Constant(SubTotal);
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("GetCartTotalWithExcludings");

			var methodCall = linq.Expression.Call(null, methodInfo, castOp, GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));

			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, subTotal) : linq.Expression.GreaterThanOrEqual(methodCall, subTotal);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}
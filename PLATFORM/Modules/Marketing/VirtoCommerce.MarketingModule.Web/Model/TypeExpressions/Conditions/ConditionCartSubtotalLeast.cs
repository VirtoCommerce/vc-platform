using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Cart subtotal is []
	public class ConditionCartSubtotalLeast : ConditionBase, IConditionExpression
	{
		public decimal SubTotal { get; set; }
		public bool Exactly { get; set; }

		#region IConditionExpression Members

		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var subTotal = linq.Expression.Constant(SubTotal);
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("GetTotalWithExcludings");
			var methodCall = linq.Expression.Call(castOp, methodInfo, GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));

			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, subTotal) : linq.Expression.GreaterThanOrEqual(methodCall, subTotal);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}
using System;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Line item subtotal is []
	public class ConditionAtCartItemExtendedTotal : ConditionBase, IConditionExpression
	{
		public decimal LineItemTotal { get; set; }

		public bool Exactly { get; set; }
		
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).IsAnyLineItemTotal(LineItemTotal, Exactly,  ExcludingCategoryIds, ExcludingProductIds)
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var lineItemTotal = linq.Expression.Constant(LineItemTotal);
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("IsAnyLineItemExtendedTotal");

			var equalsOrAtLeast = Exactly ? linq.Expression.Constant(true) : linq.Expression.Constant(false);
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, lineItemTotal, equalsOrAtLeast, GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}
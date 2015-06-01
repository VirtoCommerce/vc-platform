using System;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Promotion
{
	//[] [] items are in shopping cart
	public class ConditionAtNumItemsInCart : ConditionBase, IConditionExpression
	{
		public int NumItem { get; set; }

		public bool Exactly { get; set; }

		
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).GetCartItemsQuantity(ExcludingCategoryIds, ExcludingProductIds) > NumItem
		/// </summary>
		/// <returns></returns>
		linq.Expression<Func<IEvaluationContext, bool>> IConditionExpression.GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("GetCartItemsQuantity");
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, GetNewArrayExpression(ExcludingCategoryIds),
																	 GetNewArrayExpression(ExcludingProductIds));
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);
			return retVal;
		}

		#endregion
	}
}
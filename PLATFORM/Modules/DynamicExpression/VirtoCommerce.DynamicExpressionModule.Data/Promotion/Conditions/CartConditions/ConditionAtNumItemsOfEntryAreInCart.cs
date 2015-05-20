using System;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Promotion
{
	//[] [] items of entry are in shopping cart
	public class ConditionAtNumItemsOfEntryAreInCart : ConditionBase, IConditionExpression
	{
		public decimal NumItem { get; set; }
		public bool Exactly { get; set; }
		public string ProductId { get; set; }
		public string ProductName { get; set; }

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).GetCartItemsOfProductQuantity(ProductId, ExcludingCategoryIds, ExcludingProductIds) > NumItem
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("GetCartItemsOfProductQuantity");
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(ProductId));
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}

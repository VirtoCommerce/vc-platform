using System;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//[] [] items of category are in shopping cart
	public class ConditionAtNumItemsInCategoryAreInCart : ConditionBase, IConditionExpression
	{
	
		public decimal NumItem { get; set; }
	
		public bool Exactly { get; set; }

		public string CategoryId { get; set; }
		public string CategoryName { get; set; }
	

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).GetCartItemsOfCategoryQuantity(SelectedCategoryId, ExcludingCategoryIds, ExcludingProductIds) > NumItem
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("GetCartItemsOfCategoryQuantity");
			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(CategoryId), GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = Exactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}

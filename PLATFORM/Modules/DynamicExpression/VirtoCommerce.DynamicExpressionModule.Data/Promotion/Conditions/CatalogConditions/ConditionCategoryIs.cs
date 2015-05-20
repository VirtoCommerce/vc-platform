using System;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Promotion
{
	//Category is []
	public class ConditionCategoryIs : ConditionBase, IConditionExpression
	{
		public string CategoryId { get; set; }
		public string CategoryName { get; set; }
	
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).IsItemInCategory(SelectedCategoryId, ExcludingCategoryIds, ExcludingProductIds)
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("IsItemInCategory");

			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(CategoryId), GetNewArrayExpression(ExcludingCategoryIds),
												  GetNewArrayExpression(ExcludingProductIds));
			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		
		}

		#endregion
	}
}

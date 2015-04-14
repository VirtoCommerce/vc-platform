using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Category is []
	public class ConditionCategoryIs : ConditionBase, IConditionExpression
	{
		public string SelectedCategoryId { get; set;}
	
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).IsItemInCategory(SelectedCategoryId, ExcludingCategoryIds, ExcludingProductIds)
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContextExtension).GetMethod("IsItemInCategory");

			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(SelectedCategoryId), GetNewArrayExpression(ExcludingCategoryIds),
												  GetNewArrayExpression(ExcludingProductIds));
			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		
		}

		#endregion
	}
}

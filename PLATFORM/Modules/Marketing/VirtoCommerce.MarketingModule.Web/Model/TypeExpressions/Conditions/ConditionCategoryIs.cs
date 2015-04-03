using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Category is []
	public class ConditionCategoryIs : ConditionBase, IConditionExpression
	{
		public string SelectedCategoryId { get; set;}
	
		#region IConditionExpression Members

		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsItemInCategory");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedCategoryId), GetNewArrayExpression(ExcludingCategoryIds),
												  GetNewArrayExpression(ExcludingProductIds));
			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		
		}

		#endregion
	}
}

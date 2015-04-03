using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Product is []
	public class ConditionEntryIs : ConditionBase, IConditionExpression
	{

		public string ProductId { get; set; }
	
		#region IConditionExpression Members

		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsItemInProduct");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(ProductId));
			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}
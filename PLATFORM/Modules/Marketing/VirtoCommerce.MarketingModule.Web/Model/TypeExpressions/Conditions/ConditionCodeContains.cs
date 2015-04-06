using System;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.MarketingModule.Data;
using linq = System.Linq.Expressions;
using dataModel = VirtoCommerce.MarketingModule.Data;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Product code contains []
	public class ConditionCodeContains : ConditionBase, IConditionExpression
	{
		public string Keyword { get; set; }

		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).IsItemCodeContains(Keyword)
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(dataModel.PromotionEvaluationContext));
			var methodInfo = typeof(dataModel.PromotionEvaluationContextExtension).GetMethod("IsItemCodeContains");

			var methodCall = linq.Expression.Call(null, methodInfo, castOp, linq.Expression.Constant(Keyword));

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}

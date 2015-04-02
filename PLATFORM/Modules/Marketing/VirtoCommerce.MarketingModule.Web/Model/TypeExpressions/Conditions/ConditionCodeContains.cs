using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.MarketingModule.Data.Services;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Product code contains []
	public class ConditionCodeContains : ConditionBase, IConditionExpression
	{
		public string Keyword { get; set; }

		#region IConditionExpression Members

		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsItemCodeContains");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(Keyword));

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}

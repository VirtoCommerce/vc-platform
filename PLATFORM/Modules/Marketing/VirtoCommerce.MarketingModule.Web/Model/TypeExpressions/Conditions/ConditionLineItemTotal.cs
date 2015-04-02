using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.MarketingModule.Data.Services;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions
{
	//Line item subtotal is []
	public class ConditionLineItemTotal : ConditionBase, IConditionExpression
	{
		public decimal LineItemTotal { get; set; }

		public bool Exactly { get; set; }
		
		#region IConditionExpression Members

		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var lineItemTotal = linq.Expression.Constant(LineItemTotal);
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("IsAnyLineItemTotal");
			var equalsOrAtLeast = Exactly ? linq.Expression.Constant(true) : linq.Expression.Constant(false);
			var methodCall = linq.Expression.Call(castOp, methodInfo, lineItemTotal, equalsOrAtLeast, GetNewArrayExpression(ExcludingCategoryIds),
																	  GetNewArrayExpression(ExcludingProductIds));

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}

		#endregion
	}
}
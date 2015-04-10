using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Promotion
{
	//Currency is []
	public class ConditionCurrencyIs : ConditionBase, IConditionExpression
	{
		public string Currency { get; set; }
	
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).Currency == Currency
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IPromotionEvaluationContext), "x");
			var castOp = linq.Expression.Convert(paramX, typeof(PromotionEvaluationContext));
			var memberInfo = typeof(PromotionEvaluationContext).GetMember("Currency").First();
			var contextCurrency = linq.Expression.MakeMemberAccess(castOp, memberInfo);
			var selectedCurrency = linq.Expression.Constant(Currency);
			var binaryOp = linq.Expression.Equal(selectedCurrency, contextCurrency);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}

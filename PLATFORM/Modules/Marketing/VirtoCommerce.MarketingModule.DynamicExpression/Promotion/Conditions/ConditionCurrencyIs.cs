using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.DynamicExpression.Promotion
{
	//Currency is []
	public class ConditionCurrencyIs : ConditionBase, IConditionExpression
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes? Currency { get; set; }
	
		#region IConditionExpression Members
		/// <summary>
		/// ((PromotionEvaluationContext)x).Currency == Currency
		/// </summary>
		/// <returns></returns>
		public linq.Expression<Func<IPromotionEvaluationContext, bool>> GetConditionExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.Convert(paramX, typeof(PromotionEvaluationContext));
			var memberInfo = typeof(PromotionEvaluationContext).GetMember("Currency").First();

			var currency = linq.Expression.MakeUnary(linq.ExpressionType.Convert, linq.Expression.MakeMemberAccess(castOp, memberInfo), typeof(CurrencyCodes));  
			var selectedCurrency = linq.Expression.Constant(Currency);
			var binaryOp = linq.Expression.Equal(selectedCurrency, currency);

			var retVal = linq.Expression.Lambda<Func<IPromotionEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion
	}
}

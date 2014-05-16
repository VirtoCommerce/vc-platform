using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions
{
	[Serializable]
	public class ConditionInternetSearchedPhrase : TypedExpressionElementBase, IExpressionAdaptor
	{
		private MatchContainsStringElement _searchPhraseEl;

		public ConditionInternetSearchedPhrase(IExpressionViewModel expressionViewModel)
			: base("Shopper searched for phrase [] on the Internet".Localize(), expressionViewModel)
		{
			WithLabel("Shopper searched phrase on the Internet".Localize());
			_searchPhraseEl = WithElement(new MatchContainsStringElement(expressionViewModel)) as MatchContainsStringElement;
		}

		public string InternetSearchPhrase
		{
			get
			{
				return _searchPhraseEl.ValueString;
			}
			set
			{
				_searchPhraseEl.ValueString = value;
			}
		}

		public MatchingContains MatchCondition
		{
			get
			{
				return _searchPhraseEl.MatchingContains;
			}
		}

		public string MatchConditionValue
		{
			get
			{
				return _searchPhraseEl.MatchingContains.InputValue.ToString();
			}
			set
			{
				_searchPhraseEl.MatchingContains.InputValue = value;
			}
		}


		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			System.Linq.Expressions.ParameterExpression paramX = System.Linq.Expressions.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, paramX, typeof(PriceListAssignmentEvaluationContext));
			var propertyValue = System.Linq.Expressions.Expression.Property(castOp, typeof(PriceListAssignmentEvaluationContext).GetProperty("ShopperSearchedPhraseOnInternet"));

			MethodInfo method;
			System.Linq.Expressions.Expression methodExp;

			if (string.Equals(MatchConditionValue, MatchCondition.Contains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.Matching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.ContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.MatchingCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotMatching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase)));
			}
			else
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(InternetSearchPhrase)));
			}

			var retVal = System.Linq.Expressions.Expression.Lambda<Func<IEvaluationContext, bool>>(methodExp, paramX);

			return retVal;
		}
	}
}

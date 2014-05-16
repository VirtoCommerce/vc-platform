using System;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Controls;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	[Serializable]
	public class ConditionInternetSearchedPhrase : TypedExpressionElementBase, IExpressionAdaptor
	{
		private readonly MatchContainsStringElement _searchPhraseEl;

		public ConditionInternetSearchedPhrase(IExpressionViewModel expressionViewModel)
            : base("Shopper searched for phrase [] on the Internet".Localize(), expressionViewModel)
		{
            WithLabel("Shopper searched phrase on the Internet ".Localize());
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


		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("ShopperSearchedPhraseOnInternet"));

			MethodInfo method;
			linq.Expression methodExp;

			if (string.Equals(MatchConditionValue, MatchCondition.Contains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(InternetSearchPhrase.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.Matching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(InternetSearchPhrase.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.ContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(InternetSearchPhrase));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.MatchingCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(InternetSearchPhrase));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(InternetSearchPhrase.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotMatching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(InternetSearchPhrase.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Not(linq.Expression.Call(propertyValue, method, linq.Expression.Constant(InternetSearchPhrase)));
			}
			else
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Not(linq.Expression.Call(propertyValue, method, linq.Expression.Constant(InternetSearchPhrase)));
			}

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodExp, paramX);
			return retVal;
		}
	}
}

using System;
using System.Reflection;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions.GeoConditions
{
	[Serializable]
	public class ConditionGeoConnectionType : TypedExpressionElementBase, IExpressionAdaptor
	{
		private UserInputElement _valEl;
		private MatchingContains _matchEl;

		public ConditionGeoConnectionType(IExpressionViewModel expressionViewModel)
			: base("Are browsing from an internet connection type []".Localize(), expressionViewModel)
		{
			WithLabel("Are browsing from an internet connection type ".Localize());
			_matchEl = WithElement(new MatchingContains(false, false)) as MatchingContains;
			_valEl = WithDict(new string[] { "Ocx", "Tx", "Consumer satellite", "Frame relay", "Dsl" }, "select connection type".Localize()) as UserInputElement;
		}

		public MatchingContains MatchCondition
		{
			get
			{
				return _matchEl;
			}
		}

		public string MatchConditionValue
		{
			get
			{
				return _matchEl.InputValue.ToString();
			}
			set
			{
				_matchEl.InputValue = value;
			}
		}

		public string SelectedValue
		{
			get
			{
				return Convert.ToString(_valEl.InputValue);
			}
			set
			{
				_valEl.InputValue = value;
			}
		}

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			System.Linq.Expressions.ParameterExpression paramX = System.Linq.Expressions.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, paramX, typeof(PriceListAssignmentEvaluationContext));
			var propertyValue = System.Linq.Expressions.Expression.Property(castOp, typeof(PriceListAssignmentEvaluationContext).GetProperty("GeoConnectionType"));

			MethodInfo method;
			System.Linq.Expressions.Expression methodExp;

			if (string.Equals(MatchConditionValue, MatchCondition.Contains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedValue.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.Matching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedValue.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.ContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedValue));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.MatchingCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedValue));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedValue.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotMatching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedValue.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedValue)));
			}
			else
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedValue)));
			}

			var retVal = System.Linq.Expressions.Expression.Lambda<Func<IEvaluationContext, bool>>(methodExp, paramX);

			return retVal;
		}
	}
}

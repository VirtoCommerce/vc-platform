using System;
using System.Linq;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	[Serializable]
	public class ConditionGeoZipCode : TypedExpressionElementBase, IExpressionAdaptor
	{
		private MatchContainsStringElement _valEl;

		public ConditionGeoZipCode(IExpressionViewModel expressionViewModel)
            : base("Are browsing from zip/postal code []".Localize(), expressionViewModel)
		{
            WithLabel("Browsing from zip/postal code ".Localize());
			_valEl = WithElement(new MatchContainsStringElement(expressionViewModel)) as MatchContainsStringElement;
		}

		public string Value
		{
			get
			{
				return _valEl.ValueString;
			}
			set
			{
				_valEl.ValueString = value;
			}
		}

		public MatchingContains MatchCondition
		{
			get
			{
				return _valEl.MatchingContains;
			}
		}

		public string MatchConditionValue
		{
			get
			{
				return _valEl.MatchingContains.InputValue.ToString();
			}
			set
			{
				_valEl.MatchingContains.InputValue = value;
			}
		}


		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("GeoZipCode"));

			MethodInfo method;
			linq.Expression methodExp;

			if (string.Equals(MatchConditionValue, MatchCondition.Contains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.Matching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.ContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(Value));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.MatchingCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(Value));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotMatching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Not(linq.Expression.Call(propertyValue, method, linq.Expression.Constant(Value)));
			}
			else
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Not(linq.Expression.Call(propertyValue, method, linq.Expression.Constant(Value)));
			}

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodExp, paramX);
			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
		}
	}
}

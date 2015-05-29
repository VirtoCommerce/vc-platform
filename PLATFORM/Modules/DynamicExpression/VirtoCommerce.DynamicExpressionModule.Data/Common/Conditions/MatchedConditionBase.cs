using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
	public abstract class MatchedConditionBase<T> : DynamicExpression, IConditionExpression where T : IEvaluationContext 
	{
		public string Value { get; set; }
		public string MatchCondition { get; set; }

		private readonly string _propertyName;
		protected MatchedConditionBase(string propertyName)
		{
			_propertyName = propertyName;
		    MatchCondition = "Contains";
		}

		#region IConditionExpression Members

		public linq.Expression<Func<Domain.Common.IEvaluationContext, bool>> GetConditionExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(T));
			var propertyValue = linq.Expression.Property(castOp, typeof(T).GetProperty(_propertyName));

			MethodInfo method;
			linq.Expression methodExp = null;

			if (string.Equals(MatchCondition, "Contains", StringComparison.InvariantCultureIgnoreCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant()));
			}
			else if (string.Equals(MatchCondition, "Matching", StringComparison.InvariantCultureIgnoreCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant()));
			}
			else if (string.Equals(MatchCondition, "ContainsCase", StringComparison.InvariantCultureIgnoreCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(Value));
			}
			else if (string.Equals(MatchCondition, "MatchingCase", StringComparison.InvariantCultureIgnoreCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(Value));
			}
			else if (string.Equals(MatchCondition, "NotContains", StringComparison.InvariantCultureIgnoreCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant())));
			}
			else if (string.Equals(MatchCondition, "NotMatching"))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(Value.ToLowerInvariant())));
			}
			else if (string.Equals(MatchCondition, "NotContainsCase"))
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

		#endregion
	}
}

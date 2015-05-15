using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Content
{
	public abstract class CompareConditionBase : DynamicExpression, IConditionExpression
	{
		public string Value { get; set; }
		public string CompareCondition { get; set; }

		private readonly string _propertyName;
		protected CompareConditionBase(string propertyName)
		{
			_propertyName = propertyName;
		    CompareCondition = "IsMatching";
		}

		#region IConditionExpression Members

		public Expression<Func<Domain.Common.IEvaluationContext, bool>> GetConditionExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("_propertyName"));

			var valueExpression = linq.Expression.Constant(ParseString(Value));
			linq.BinaryExpression binaryOp;

			if (String.Equals(CompareCondition, "IsMatching", StringComparison.InvariantCultureIgnoreCase))
				binaryOp = linq.Expression.Equal(propertyValue, valueExpression);
			else if (String.Equals(CompareCondition, "IsNotMatching", StringComparison.InvariantCultureIgnoreCase))
				binaryOp = linq.Expression.NotEqual(propertyValue, valueExpression);
			else if (String.Equals(CompareCondition, "IsGreaterThan", StringComparison.InvariantCultureIgnoreCase))
				binaryOp = linq.Expression.GreaterThan(propertyValue, valueExpression);
			else if (String.Equals(CompareCondition, "IsGreaterThanOrEqual", StringComparison.InvariantCultureIgnoreCase))
				binaryOp = linq.Expression.GreaterThanOrEqual(propertyValue, valueExpression);
			else if (String.Equals(CompareCondition, "IsLessThan", StringComparison.InvariantCultureIgnoreCase))
				binaryOp = linq.Expression.LessThan(propertyValue, valueExpression);
			else
				binaryOp = linq.Expression.LessThanOrEqual(propertyValue, valueExpression);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		#endregion

		private object ParseString(string str)
		{
			int intValue;
			double doubleValue;
			char charValue;
			bool boolValue;
			TimeSpan timespan;
			DateTime dateTime;

			// Place checks higher if if-else statement to give higher priority to type.
			if (int.TryParse(str, out intValue))
				return intValue;
			else if (double.TryParse(str, out doubleValue))
				return doubleValue;
			else if (TimeSpan.TryParse(str, out timespan))
				return timespan;
			else if (DateTime.TryParse(str, out dateTime))
				return dateTime;
			else if (char.TryParse(str, out charValue))
				return charValue;
			else if (bool.TryParse(str, out boolValue))
				return boolValue;

			return null;
		}
	}
}

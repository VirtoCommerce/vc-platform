using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;


namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	[Serializable]
	public class ConditionGeoTimeZone : TypedExpressionElementBase, IExpressionAdaptor
	{
		private UserInputElement _valEl;

		public ConditionGeoTimeZone(IExpressionViewModel expressionViewModel)
            : base("Are browsing from a time zone -/+ offset from UTC []".Localize(), expressionViewModel)
		{
            WithLabel("Are browsing from a time zone -/+ offset from UTC ".Localize());
			CompareConditions = WithElement(new CompareConditions(true)) as CompareConditions;
            _valEl = WithCustomDict(GetTimeZonesArray(), new string[] { "select time zone".Localize() }) as UserInputElement;
		}

		public string[] GetTimeZonesArray()
		{
			var retVal = new List<string>();
			TimeZoneInfo.GetSystemTimeZones().OrderBy(x => x.BaseUtcOffset).ToList().ForEach(x => retVal.Add(x.DisplayName));
			return retVal.ToArray();
		}

		public CompareConditions CompareConditions
		{
			get;
			set;
		}

		public string CompareConditionsValue
		{
			get { return CompareConditions.InputValue.ToString(); }
			set { CompareConditions.InputValue = value; }
		}

		public TimeSpan SelectedValue
		{
			get
			{
				return TimeZoneInfo.GetSystemTimeZones().First(x => x.DisplayName == _valEl.InputValue.ToString()).BaseUtcOffset;
			}
			set
			{
				_valEl.InputValue = value;
			}
		}

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("GeoTimeZone"));

			var conditionSelectedTimeZone = linq.Expression.Constant(SelectedValue);
			linq.BinaryExpression binaryOp;

			if (CompareConditions.IsMatching)
				binaryOp = linq.Expression.Equal(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsNotMatching)
				binaryOp = linq.Expression.NotEqual(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsGreaterThan)
				binaryOp = linq.Expression.GreaterThan(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsGreaterThanOrEqual)
				binaryOp = linq.Expression.GreaterThanOrEqual(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsLessThan)
				binaryOp = linq.Expression.LessThan(propertyValue, conditionSelectedTimeZone);
			else
				binaryOp = linq.Expression.LessThanOrEqual(propertyValue, conditionSelectedTimeZone);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}
	}
}

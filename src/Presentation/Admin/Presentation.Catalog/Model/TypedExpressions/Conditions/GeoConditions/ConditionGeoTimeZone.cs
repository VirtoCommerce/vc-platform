using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions.GeoConditions
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

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			System.Linq.Expressions.ParameterExpression paramX = System.Linq.Expressions.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, paramX, typeof(PriceListAssignmentEvaluationContext));
			var propertyValue = System.Linq.Expressions.Expression.Property(castOp, typeof(PriceListAssignmentEvaluationContext).GetProperty("GeoTimeZone"));

			var conditionSelectedTimeZone = System.Linq.Expressions.Expression.Constant(SelectedValue);
			System.Linq.Expressions.BinaryExpression binaryOp;

			if (CompareConditions.IsMatching)
				binaryOp = System.Linq.Expressions.Expression.Equal(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsNotMatching)
				binaryOp = System.Linq.Expressions.Expression.NotEqual(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsGreaterThan)
				binaryOp = System.Linq.Expressions.Expression.GreaterThan(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsGreaterThanOrEqual)
				binaryOp = System.Linq.Expressions.Expression.GreaterThanOrEqual(propertyValue, conditionSelectedTimeZone);
			else if (CompareConditions.IsLessThan)
				binaryOp = System.Linq.Expressions.Expression.LessThan(propertyValue, conditionSelectedTimeZone);
			else
				binaryOp = System.Linq.Expressions.Expression.LessThanOrEqual(propertyValue, conditionSelectedTimeZone);

			var retVal = System.Linq.Expressions.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}
	}
}

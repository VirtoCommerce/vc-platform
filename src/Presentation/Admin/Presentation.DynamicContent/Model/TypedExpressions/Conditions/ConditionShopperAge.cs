using System;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Controls;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	[Serializable]
	public class ConditionShopperAge : TypedExpressionElementBase, IExpressionAdaptor
	{
		private readonly AgeElement _ageEl;

		public ConditionShopperAge(IExpressionViewModel expressionViewModel)
            : base("Shopper age []".Localize(), expressionViewModel)
		{
            WithLabel("Shopper age is ".Localize());
			_ageEl = WithElement(new AgeElement(expressionViewModel)) as AgeElement;
		}

		public int Age
		{
			get
			{
				return _ageEl.Age;
			}
			set
			{
				_ageEl.Age = value;
			}
		}


		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("ShopperAge"));

			var conditionAge = linq.Expression.Constant(Age);
			linq.BinaryExpression binaryOp;

			if (_ageEl.CompareConditions.IsMatching)
				binaryOp = linq.Expression.Equal(propertyValue, conditionAge);
			else if (_ageEl.CompareConditions.IsNotMatching)
				binaryOp = linq.Expression.NotEqual(propertyValue, conditionAge);
			else if (_ageEl.CompareConditions.IsGreaterThan)
				binaryOp = linq.Expression.GreaterThan(propertyValue, conditionAge);
			else if (_ageEl.CompareConditions.IsGreaterThanOrEqual)
				binaryOp = linq.Expression.GreaterThanOrEqual(propertyValue, conditionAge);
			else if (_ageEl.CompareConditions.IsLessThan)
				binaryOp = linq.Expression.LessThan(propertyValue, conditionAge);
			else
				binaryOp = linq.Expression.LessThanOrEqual(propertyValue, conditionAge);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}
	}
}

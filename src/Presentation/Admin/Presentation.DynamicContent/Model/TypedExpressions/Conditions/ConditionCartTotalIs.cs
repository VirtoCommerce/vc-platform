using System;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	[Serializable]
	public class ConditionCartTotalIs : TypedExpressionElementBase, IExpressionAdaptor
	{
		private readonly CartTotalElement _cartTotalEl;

		public ConditionCartTotalIs(IExpressionViewModel expressionViewModel)
            : base("Cart total $ []".Localize(), expressionViewModel)
		{
            WithLabel("Cart total is $ ".Localize());
			_cartTotalEl = WithElement(new CartTotalElement(expressionViewModel)) as CartTotalElement;
		}

		public decimal CartTotal
		{
			get
			{
				return _cartTotalEl.NumVal;
			}
			set
			{
				_cartTotalEl.NumVal = value;
			}
		}


		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("CartTotal"));
						
			var conditionTotal = linq.Expression.Constant(CartTotal);

			linq.BinaryExpression binaryOp;
			if (_cartTotalEl.CompareConditions.IsMatching)
				binaryOp = linq.Expression.Equal(propertyValue, conditionTotal);
			else if (_cartTotalEl.CompareConditions.IsNotMatching)
				binaryOp = linq.Expression.NotEqual(propertyValue, conditionTotal);
			else if (_cartTotalEl.CompareConditions.IsGreaterThan)
				binaryOp = linq.Expression.GreaterThan(propertyValue, conditionTotal);
			else if (_cartTotalEl.CompareConditions.IsGreaterThanOrEqual)
				binaryOp = linq.Expression.GreaterThanOrEqual(propertyValue, conditionTotal);
			else if (_cartTotalEl.CompareConditions.IsLessThan)
				binaryOp = linq.Expression.LessThan(propertyValue, conditionTotal);
			else
				binaryOp = linq.Expression.LessThanOrEqual(propertyValue, conditionTotal);
			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}
	}
}

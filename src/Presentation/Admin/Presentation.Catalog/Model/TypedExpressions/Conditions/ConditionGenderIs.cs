using System;
using System.Linq;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions
{
    [Serializable]
	public class ConditionGenderIs : TypedExpressionElementBase, IExpressionAdaptor
    {
		public ConditionGenderIs(IExpressionViewModel expressionViewModel)
			: base("Shopper is of gender []".Localize(), expressionViewModel)
		{
			WithLabel("Shopper gender is ".Localize());
			_gender = WithElement(new GenderElement()) as GenderElement;
		}

		private GenderElement _gender;
		public string Gender 
		{
			get { return _gender.InputValue.ToString(); }
			set { _gender.InputValue = value; }
		}

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PriceListAssignmentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(PriceListAssignmentEvaluationContext).GetProperty("ShopperGender"));

			var conditionAge = linq.Expression.Constant(Gender);
			var binaryOp = linq.Expression.Equal(propertyValue, conditionAge);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}
    }
}

using System;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
	public class ConditionGenderIs : TypedExpressionElementBase, IExpressionAdaptor
    {
		public ConditionGenderIs(IExpressionViewModel expressionViewModel)
            : base("Shopper gender is []".Localize(), expressionViewModel)
		{
            WithLabel("Shopper gender is ".Localize());
			_gender = WithElement(new GenderElement()) as GenderElement;
		}

		private readonly GenderElement _gender;
		public string Gender
		{
			get { return _gender.InputValue.ToString(); }
			set { _gender.InputValue = value; }
		}

		

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("ShopperGender"));

			var conditionAge = linq.Expression.Constant(Gender);
			var binaryOp = linq.Expression.Equal(propertyValue, conditionAge);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);
			return retVal;
		}
    }
}

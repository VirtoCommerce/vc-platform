using System;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
	[Serializable]
	public class ConditionItemTypeIs : TypedExpressionElementBase, IExpressionAdaptor
	{
		public ConditionItemTypeIs(IExpressionViewModel expressionViewModel)
			: base("Item is of type []".Localize(), expressionViewModel)
		{
			WithLabel("Item type is ".Localize());
			_itemType = WithElement(new ItemTypeElement()) as ItemTypeElement;
		}

		private ItemTypeElement _itemType;
		public string ItemType
		{
			get { return _itemType.InputValue.ToString(); }
			set { _itemType.InputValue = value; }
		}

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			linq.ParameterExpression paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DisplayTemplateEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DisplayTemplateEvaluationContext).GetProperty("ItemType"));

			var conditionItemType = linq.Expression.Constant(ItemType);
			var binaryOp = linq.Expression.Equal(propertyValue, conditionItemType);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
		}
	}
}

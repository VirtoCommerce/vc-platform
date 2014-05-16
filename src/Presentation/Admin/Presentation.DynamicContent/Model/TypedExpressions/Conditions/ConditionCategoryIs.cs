using System;
using linq = System.Linq.Expressions;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	/// <summary>
	/// Condition to select category with or without subcategories
	/// </summary>
	[Serializable]
	public class ConditionCategoryIs : TypedExpressionElementBase, IExpressionAdaptor
	{
		private readonly CategoryElement _itemsInCategoryEl;
		private readonly bool _useSubCategories;

		/// <summary>
		/// Condition to select category with or without subcategories
		/// </summary>
		/// <param name="expressionViewModel">viewmodel that implements interface IExpressionViewModel</param>
		/// <param name="useSubcategories">Use true if the condition should use subcategories otherwise - false</param>
		public ConditionCategoryIs(IExpressionViewModel expressionViewModel, bool useSubcategories)
            : base(useSubcategories ? "Current category is [] or its subcategory".Localize() : "Current category is []".Localize(), expressionViewModel)
		{
            WithLabel("Shopper is in category ".Localize());
			_itemsInCategoryEl = WithElement(new CategoryElement(expressionViewModel)) as CategoryElement;
			_useSubCategories = useSubcategories;
			if (useSubcategories)
                WithLabel(" or subcategories".Localize());
		}

		public string SelectedCategoryId
		{
			get
			{
				return _itemsInCategoryEl.SelectedCategoryId;
			}
			set
			{
				_itemsInCategoryEl.SelectedCategoryId = value;
			}
		}


		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var methodInfo = _useSubCategories ? 
				typeof(DynamicContentEvaluationContext).GetMethod("IsShopperInCategoryOrSubcategories") : 
				typeof(DynamicContentEvaluationContext).GetMethod("IsShopperInCategory");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedCategoryId));
			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodCall, paramX);

			return retVal;
		}
	}
}

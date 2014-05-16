using System;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionAtNumItemsOfEntryAreInCart : PromotionExpressionBlock
	{
		private readonly UserInputElement _numItemEl;
		private readonly ItemsInEntry _itemsInEntryEl;

		public ConditionAtNumItemsOfEntryAreInCart(IExpressionViewModel expressionViewModel)
			: base("[] [] items of entry are in shopping cart".Localize(), expressionViewModel)
		{
			ExactlyLeast = WithElement(new ExactlyLeast()) as ExactlyLeast;
			_numItemEl = WithUserInput(1, 0) as UserInputElement;
			_itemsInEntryEl = WithElement(new ItemsInEntry(expressionViewModel)) as ItemsInEntry;
			WithLabel("are in shopping cart".Localize());
			_itemsInEntryEl.SelectedItemChanged += SelectedEntryItemChanged;
			InitializeExcludings();
		}

		public decimal NumItem
		{
			get
			{
				return Convert.ToDecimal(_numItemEl.InputValue);
			}
			set
			{
				_numItemEl.InputValue = value;
			}
		}

		public ExactlyLeast ExactlyLeast { get; set; }

		public string SelectedItemId
		{
			get
			{
				return _itemsInEntryEl.SelectedItemId;
			}
			set
			{
				_itemsInEntryEl.SelectedItemId = value;
			}
		}


		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var methodInfo = typeof(PromotionEvaluationContext).GetMethod("GetItemsOfProductQuantity");
			var methodCall = linq.Expression.Call(castOp, methodInfo, linq.Expression.Constant(SelectedItemId), ExcludingSkuIds.GetNewArrayExpression());
			var numItem = linq.Expression.Constant(NumItem);
			var binaryOp = ExactlyLeast.IsExactly ? linq.Expression.Equal(methodCall, numItem) : linq.Expression.GreaterThanOrEqual(methodCall, numItem);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
			InitializeExcludings();
		}

		public int SelectedItemType { get; set; }

		private void SelectedEntryItemChanged(int newItemType)
		{
			SelectedItemType = newItemType;
			InitializeExcludings();
		}

		private void InitializeExcludings()
		{
			// Only products can have excludings and those can be SKUs only.
			if (SelectedItemType == (int)ItemSubtype.Product)
				WithAvailableExcluding(() => new ItemsInSku(ExpressionViewModel));
			else
				DisableExcludings();
		}
	}
}

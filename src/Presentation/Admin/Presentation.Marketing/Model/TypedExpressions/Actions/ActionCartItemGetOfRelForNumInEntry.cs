using System;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ActionCartItemGetOfRelForNumInEntry : PromotionExpressionBlock
	{
		private readonly UserInputElement _numItemEl;
		private readonly UserInputElement _amountEl;
		private readonly ItemsInEntry _itemsInEntryEl;

		public ActionCartItemGetOfRelForNumInEntry(IExpressionViewModel expressionViewModel)
			: base("Get [] % off [] items of entry []".Localize(), expressionViewModel)
		{
			WithLabel("Get".Localize());
			_amountEl = WithUserInput(0, 0, 100) as UserInputElement;
			WithLabel(" % off".Localize());
			_numItemEl = WithUserInput(1, 0) as UserInputElement;
			_itemsInEntryEl = WithElement(new ItemsInEntry(expressionViewModel)) as ItemsInEntry;
			_itemsInEntryEl.SelectedItemChanged += SelectedEntryItemChanged;
			InitializeExcludings();
		}

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

		public decimal Amount
		{
			get
			{
				return Convert.ToDecimal(_amountEl.InputValue);
			}
			set
			{
				_amountEl.InputValue = value;
			}
		}

		public int NumItem
		{
			get
			{
				return Convert.ToInt32(_numItemEl.InputValue);
			}
			set
			{
				_numItemEl.InputValue = value;
			}
		}


		public override PromotionReward[] GetPromotionRewards()
		{
			var retVal = new CatalogItemReward
			{
				Amount = Amount,
				QuantityLimit = NumItem,
				AmountTypeId = (int)RewardAmountType.Relative,
				ProductId = SelectedItemId,
				ExcludingSkus = String.Join(";", ExcludingSkuIds)
			};
			return new PromotionReward[] { retVal };
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
			// Only products can have excludings and they can be SKUs only.
			if (SelectedItemType == (int)ItemSubtype.Product)
				WithAvailableExcluding(() => new ItemsInSku(ExpressionViewModel));
			else
				DisableExcludings();
		}
	}
}

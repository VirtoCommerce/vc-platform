using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ItemsInEntry : TypedExpressionElementBase
	{
		private readonly CustomSelectorElement _valueSelectorEl;
		private const string ElementLabel = "Items of entry";
		private const string SelectLabel = "select entry";
		private const string Prefix = "items of entry";

		internal delegate void SelectedItemChange(int newItemType);
		internal event SelectedItemChange SelectedItemChanged;


		public ItemsInEntry(IExpressionViewModel expressionViewModel)
			: base(ElementLabel, expressionViewModel)
		{
			WithLabel(Prefix);
			_valueSelectorEl = WithCustomSelect(ValueSelector, SelectLabel) as CustomSelectorElement;
		}

		public string SelectedItemId
		{
			get
			{
				return Convert.ToString(_valueSelectorEl.InputValue);
			}
			set
			{
				_valueSelectorEl.InputValue = value;
			}
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
		{
			base.InitializeAfterDeserialized(expressionViewModel);
			_valueSelectorEl.ValueSelector = ValueSelector;
		}

		private Func<object> ValueSelector
		{
			get
			{
				return () =>
				{
					var catalogId = string.IsNullOrEmpty(((IPromotionViewModel)ExpressionViewModel).CatalogId) ? string.Empty : ((IPromotionViewModel)ExpressionViewModel).CatalogId;

					var itemVM = ((IPromotionViewModel)ExpressionViewModel).SearchItemVmFactory.GetViewModelInstance(
						new KeyValuePair<string, object>("catalogInfo", catalogId),
						new KeyValuePair<string, object>("searchType", string.Empty)
						);
					((IPromotionViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
						new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select an Entry".Localize() },
						(x) =>
						{
							if (x.Confirmed)
							{
								SelectedItemId = itemVM.SelectedItem.ItemId;
								_valueSelectorEl.InputDisplayName = itemVM.SelectedItem.Name;

								if (SelectedItemChanged != null)
									SelectedItemChanged(ItemSubtypeHelper.GetItemTypeInt(itemVM.SelectedItem));

								// fake assignment to change IsModified = true
								((IPromotionViewModel)ExpressionViewModel).InnerItem.Name = ((IPromotionViewModel)ExpressionViewModel).InnerItem.Name;
							}
						});

					return SelectedItemId;
				};

			}
		}
	}
}

using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
	[Serializable]
	public class ItemSelectElement : TypedExpressionElementBase
	{
		private CustomSelectorElement _valueSelectorEl;
		private static readonly string ElementLabel = "Items of entry".Localize();
		private static readonly string SelectLabel = "select item".Localize();

		internal delegate void SelectedItemChange(int newItemType);
		internal event SelectedItemChange SelectedItemChanged;


		public ItemSelectElement(IExpressionViewModel expressionViewModel)
			: base(ElementLabel, expressionViewModel)
		{
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
					// ParameterOverride ParameterValue cannot be null
					var catalogId = string.Empty;
					var itemVM = ((IDisplayTemplateViewModel)ExpressionViewModel).SearchItemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("searchType", string.Empty));
					((IDisplayTemplateViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
						new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select an Entry".Localize() },
						(x) =>
						{
							if (x.Confirmed && itemVM.SelectedItem != null)
							{
								SelectedItemId = itemVM.SelectedItem.ItemId;
								_valueSelectorEl.InputDisplayName = itemVM.SelectedItem.Name;

								if (SelectedItemChanged != null)
									SelectedItemChanged(ItemSubtypeHelper.GetItemTypeInt(itemVM.SelectedItem));

								// fake assignment to change IsModified = true
								((IDisplayTemplateViewModel)ExpressionViewModel).InnerItem.Name = ((IDisplayTemplateViewModel)ExpressionViewModel).InnerItem.Name;
							}
						});

					return SelectedItemId;
				};

			}
		}
	}
}

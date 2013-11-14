using System;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
    [Serializable]
    public class ItemSelectElement : TypedExpressionElementBase
    {
        private CustomSelectorElement _valueSelectorEl;
        private const string ElementLabel = "Items of entry";
        private const string SelectLabel = "select item";

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
                        new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select an Entry" },
                        (x) =>
                        {
                            if (x.Confirmed)
                            {
                                SelectedItemId = itemVM.SelectedItem.ItemId;
                                _valueSelectorEl.InputDisplayName = itemVM.SelectedItem.Name;

                                if (SelectedItemChanged != null)
                                    SelectedItemChanged(ItemSubtypeHelper.GetItemTypeInt(itemVM.SelectedItem));

                                // fake asignment to change IsModified = true
								((IDisplayTemplateViewModel)ExpressionViewModel).InnerItem.Name = ((IDisplayTemplateViewModel)ExpressionViewModel).InnerItem.Name;
                            }
                        });

                    return SelectedItemId;
                };

            }
        }
    }
}

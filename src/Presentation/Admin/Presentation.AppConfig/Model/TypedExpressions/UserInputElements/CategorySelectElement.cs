using System;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
    [Serializable]
    public class CategorySelectElement : TypedExpressionElementBase
    {
        private CustomSelectorElement _valueSelectorEl;
        private static readonly string ElementLabel = "Items of category".Localize();
        private static readonly string SelectLabel = "select category".Localize();

        public CategorySelectElement(IExpressionViewModel expressionViewModel)
            : base(ElementLabel, expressionViewModel)
        {
            _valueSelectorEl = WithCustomSelect(ValueSelector, SelectLabel) as CustomSelectorElement;
        }

        public CategorySelectElement(string elementLabel, IExpressionViewModel expressionViewModel)
            : base(elementLabel, expressionViewModel)
        {
            _valueSelectorEl = WithCustomSelect(ValueSelector, SelectLabel) as CustomSelectorElement;
        }

        public string SelectedCategoryId
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

        private Func<object> ValueSelector
        {
            get
            {
                return () =>
                {
                    var itemVM = ((IDisplayTemplateViewModel)ExpressionViewModel).SearchCategoryVmFactory.GetViewModelInstance();
                    //itemVM.SearchModifier = SearchCategoryModifier.RealCatalogsOnly;
                    itemVM.InitializeForOpen();
                    ((IDisplayTemplateViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
                        new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select Category".Localize() },
                        (x) =>
                        {
                            if (x.Confirmed)
                            {
                                var category = itemVM.SelectedItem;
                                SelectedCategoryId = category.CategoryId;
                                _valueSelectorEl.InputDisplayName = category.Name;
                            }
                        });

                    return SelectedCategoryId;
                };
            }
        }

        public override void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
        {
            base.InitializeAfterDeserialized(expressionViewModel);
            _valueSelectorEl.ValueSelector = ValueSelector;
        }
    }
}

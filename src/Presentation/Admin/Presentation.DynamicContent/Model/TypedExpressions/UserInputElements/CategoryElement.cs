using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public class CategoryElement : TypedExpressionElementBase
    {
        private readonly CustomSelectorElement _valueSelectorEl;
        private const string ElementLabel = "Current category";
        private const string SelectLabel = "select category";

        public CategoryElement(IExpressionViewModel expressionViewModel)
            : base(ElementLabel, expressionViewModel)
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
                    var itemVM = ((IContentPublishingItemViewModel)ExpressionViewModel).SearchCategoryVmFactory.GetViewModelInstance();
                    //itemVM.SearchModifier = SearchCategoryModifier.RealCatalogsOnly;
                    itemVM.InitializeForOpen();
                    ((IContentPublishingItemViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
                        new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select Category".Localize() },
                        x =>
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
    }
}

using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
    public class ItemsInCategory : TypedExpressionElementBase
    {
        private readonly CustomSelectorElement _valueSelectorEl;

        private const string ElementLabel = "Items of category";
        private const string SelectLabel = "select category";
        private const string Prefix = "items of category";

        public ItemsInCategory(IExpressionViewModel expressionViewModel)
            : base(ElementLabel, expressionViewModel)
        {
            WithLabel(Prefix);
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
                    var catalogId = string.IsNullOrEmpty(((IPromotionViewModel)ExpressionViewModel).CatalogId) ? string.Empty : ((IPromotionViewModel)ExpressionViewModel).CatalogId;
                    var itemVM = ((IPromotionViewModel)ExpressionViewModel).SearchCategoryVmFactory.GetViewModelInstance(
                        new KeyValuePair<string, object>("catalogInfo", catalogId)
                        );
                    // itemVM.SearchModifier = SearchCategoryModifier.RealCatalogsOnly;
                    itemVM.InitializeForOpen();
                    ((IPromotionViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
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

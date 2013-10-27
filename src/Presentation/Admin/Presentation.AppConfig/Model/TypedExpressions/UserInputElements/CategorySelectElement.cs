using System;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
    [Serializable]
    public class CategorySelectElement : TypedExpressionElementBase
    {
        private CustomSelectorElement _valueSelectorEl;
        private const string ElementLabel = "Items of category";
        private const string SelectLabel = "select category";

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
					var catalogId = string.Empty;
					var itemVM = ((IDisplayTemplateViewModel)ExpressionViewModel).SearchCategoryVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("searchType", string.Empty));
					itemVM.SearchModifier = SearchCategoryModifier.RealCatalogsOnly;
					itemVM.InitializeForOpen();
					((IDisplayTemplateViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
						new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select Category" },
						(x) =>
						{
							if (x.Confirmed)
							{
								var category = (Category)itemVM.SelectedItem;
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

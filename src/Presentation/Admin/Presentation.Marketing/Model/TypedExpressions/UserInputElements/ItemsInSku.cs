using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ItemsInSku : TypedExpressionElementBase
	{
		private readonly CustomSelectorElement _valueSelectorEl;
		private const string ElementLabel = "Items of SKU";
		private const string SelectLabel = "select SKU";
		private const string Prefix = "items of SKU";

		public ItemsInSku(IExpressionViewModel expressionViewModel)
			: base(ElementLabel, expressionViewModel)
		{
			WithLabel(Prefix);
			_valueSelectorEl = WithCustomSelect(ValueSelector, SelectLabel) as CustomSelectorElement;
		}

		public string SelectedSkuId
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
						new KeyValuePair<string, object>("searchType", "Variation")
						);
					((IPromotionViewModel)ExpressionViewModel).CommonConfirmRequest.Raise(
						new ConditionalConfirmation(() => itemVM.SelectedItem != null) { Content = itemVM, Title = "Select a SKU".Localize() },
						(x) =>
						{
							if (x.Confirmed)
							{
								var item = (Sku)itemVM.SelectedItem;
								SelectedSkuId = item.ItemId;
								_valueSelectorEl.InputDisplayName = item.Name;

								// fake assignment to change IsModified = true
								((IPromotionViewModel)ExpressionViewModel).InnerItem.Name = ((IPromotionViewModel)ExpressionViewModel).InnerItem.Name;
							}
						});

					return SelectedSkuId;
				};

			}
		}
	}
}

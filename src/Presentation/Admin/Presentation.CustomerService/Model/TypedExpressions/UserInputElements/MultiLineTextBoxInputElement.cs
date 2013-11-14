using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.Model
{
	[Serializable]
	public class MultiLineTextBoxInputElement : TypedExpressionElementBase
	{
		private readonly CustomSelectorElement _valueSelectorEl;
		private readonly string ElementLabel;
		private readonly string SelectLabel;
		private readonly string _defaultValue;

		public MultiLineTextBoxInputElement(IExpressionViewModel expressionViewModel, string elementLabel, string selectLabel, string defaultValue)
			: base(elementLabel, expressionViewModel)
		{
			ElementLabel = elementLabel;
			SelectLabel = selectLabel;
			_defaultValue = defaultValue;
			_valueSelectorEl = WithCustomSelect(ValueSelector, SelectLabel) as CustomSelectorElement;
		}

		public string InputValue
		{
			get
			{
				return Convert.ToString(_valueSelectorEl.InputValue);
			}
			set
			{
				_valueSelectorEl.InputValue = value;
				_valueSelectorEl.InputDisplayName = string.Concat(value.Replace(Environment.NewLine, string.Empty).Substring(0, 20), "...");
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
					var itemVM = ((ICaseRuleViewModel)ExpressionViewModel).MultiLineEditVmFactory.GetViewModelInstance(

						new KeyValuePair<string, object>("title", ElementLabel),
						new KeyValuePair<string, object>("subTitle", SelectLabel));

					itemVM.InnerItem = InputValue == SelectLabel ? _defaultValue : InputValue;

					return InputValue;
				};
			}
		}
	}
}

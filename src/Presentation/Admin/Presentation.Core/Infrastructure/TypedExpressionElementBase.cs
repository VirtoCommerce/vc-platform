using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    [Serializable]
	public abstract class TypedExpressionElementBase : CompositeElement
    {
        private readonly CompositeElement _exludingEl;

	    protected TypedExpressionElementBase(string displayName, IExpressionViewModel expressionViewModel)
        {
            DisplayName = displayName;

            _exludingEl = new CompositeElement();
            _exludingEl.HeaderElements.Add(new LabelElement { Label = "excluding".Localize() });
            _exludingEl.NewChildLabel = "+ excluding".Localize();
			ExpressionViewModel = expressionViewModel;
        }

        #region Properties
        [NonSerialized]
		private IExpressionViewModel _expressionViewModel;
		public IExpressionViewModel ExpressionViewModel
        {
			get { return _expressionViewModel; }
			set { _expressionViewModel = value; }
        }

        protected CompositeElement[] AllExcludingElements
        {
            get
            {
                return _exludingEl.Children.OfType<CompositeElement>().ToArray();
            }
        }
		
        #endregion

		public virtual void InitializeAfterDeserialized(IExpressionViewModel expressionViewModel)
        {
			ExpressionViewModel = expressionViewModel;

            foreach (var item in HeaderElements.OfType<TypedExpressionElementBase>())
            {
				item.InitializeAfterDeserialized(expressionViewModel);
            }

            foreach (var item in _exludingEl.Children.OfType<TypedExpressionElementBase>())
            {
				item.InitializeAfterDeserialized(expressionViewModel);
            }
            _exludingEl.NewChildLabel = "+ excluding".Localize();

            foreach (var item in Children.OfType<TypedExpressionElementBase>())
            {
				item.InitializeAfterDeserialized(expressionViewModel);
            }
        }

        protected ExpressionElement WithElement(ExpressionElement el)
        {
            HeaderElements.Add(el);
            return el;
        }

        protected ExpressionElement WithLabel(string label)
        {
            var labelEl = new LabelElement { Label = label };
            return WithElement(labelEl);
        }

        protected ExpressionElement WithDict(string[] values, string defaultValue)
        {
            var dictEl = new DictionaryElement { ValueType = typeof(string), AvailableValues = values, DefaultValue = defaultValue };
            return WithElement(dictEl);
        }

		protected ExpressionElement WithCustomDict(string[] values, string[] defaultValue)
		{
			var dictEl = new CustomDictionaryElement { ValueType = typeof(string), AvailableValues = values, DefaultValue = defaultValue };
			return WithElement(dictEl);
		}

        protected ExpressionElement WithUserInput<T>(T defaultValue)
        {
	        if (typeof (T) == typeof (int))
		        return WithUserInput(Convert.ToInt32(defaultValue), int.MinValue, int.MaxValue);

			if (typeof(T) == typeof(double))
				return WithUserInput(Convert.ToDouble(defaultValue), double.MinValue, double.MaxValue);

			if (typeof(T) == typeof(decimal))
				return WithUserInput(Convert.ToDecimal(defaultValue), decimal.MinValue, decimal.MaxValue);
	        
		    var inputEl = new UserInputElement {ValueType = typeof (T), DefaultValue = defaultValue};
		    return WithElement(inputEl);
        }

		protected ExpressionElement WithUserInput<T>(T defaultValue, T minValue)
		{
			if (typeof(T) == typeof(int))
				return WithUserInput(Convert.ToInt32(defaultValue), Convert.ToInt32(minValue), int.MaxValue);

			return typeof(T) == typeof(double) ? WithUserInput(Convert.ToDouble(defaultValue), Convert.ToDouble(minValue), double.MaxValue) :
				WithUserInput(Convert.ToDecimal(defaultValue), Convert.ToDecimal(minValue), decimal.MaxValue);
		}

		protected ExpressionElement WithUserInput<T>(T defaultValue, T minValue, T maxValue)
		{
			var inputEl = new UserInputElement { ValueType = typeof(T), MinValue = minValue, MaxValue = maxValue, DefaultValue = defaultValue };
			return WithElement(inputEl);
		}

        protected ExpressionElement WithCustomSelect(Func<object> selector, string defaultValue = "select")
        {
            var customSelectEl = new CustomSelectorElement { ValueSelector = selector, DefaultValue = defaultValue, ValueType = typeof(string) };
            return WithElement(customSelectEl);
        }

        protected void AddExludingElement(CompositeElement exludingEl)
        {
            _exludingEl.Children.Add(exludingEl);
        }

        public void WithAvailabeChildren(IEnumerable<Func<CompositeElement>> elementGetters)
        {
            this.AvailableChildrenGetters.AddRange(elementGetters);
        }

        protected void DisableExcludings()
        {
            _exludingEl.Children.Clear();
            if (HeaderElements.Contains(_exludingEl))
            {
                HeaderElements.Remove(_exludingEl);
            }
        }

        protected void WithAvailableExcluding(Func<CompositeElement> elementGetter)
        {
            if (!_exludingEl.AvailableChildrenGetters.Contains(elementGetter))
                _exludingEl.AvailableChildrenGetters.Add(elementGetter);

            if (!HeaderElements.Contains(_exludingEl))
            {
                HeaderElements.Add(_exludingEl);
            }
        }
	}
}

using System;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public abstract class KeyValueDictionaryElement : UserInputElement
    {
        [NonSerialized]
        KeyValuePair<string, string>[] _availableValues;

        public KeyValuePair<string, string>[] AvailableValues
        {
            get { return _availableValues; }
            set { _availableValues = value; OnPropertyChanged("AvailableValues"); }
        }

        public new KeyValuePair<string, string> DefaultValue { get; set; }

		public abstract void InitializeAvailableValues(IExpressionViewModel expressionViewModel);
    }
}

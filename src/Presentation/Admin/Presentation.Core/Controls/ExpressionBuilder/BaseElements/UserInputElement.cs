using System;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class UserInputElement : ExpressionElement
    {
        public Type ValueType;

        private object _inputValue;
        public object InputValue
        {
            get { return _inputValue ?? DefaultValue; }
            set
            {
                if (ValueType == typeof(string))
                {
                    _inputValue = Convert.ToString(value);
                    OnPropertyChanged("InputValue");
                }
                else
                {
                    // calling "TryParse" dynamically
                    var methodInfo = ValueType.GetMethod(
                        "TryParse",
                        BindingFlags.Public | BindingFlags.Static,
                        Type.DefaultBinder,
                        new[] { typeof(string), ValueType.MakeByRefType() },
                        null
                    );

                    if (methodInfo != null)
                    {
                        var inputParameters = new object[] { Convert.ToString(value), null };
                        var invokeResult = (bool)methodInfo.Invoke(null, inputParameters);

                        if (invokeResult) _inputValue = inputParameters[1];
                    }
					OnPropertyChanged("InputValue");
                }
            }
        }

        private string _inputDisplayName;
        public string InputDisplayName
        {
            get { return _inputDisplayName ?? Convert.ToString(DefaultValue); }
            set { _inputDisplayName = value; OnPropertyChanged("InputDisplayName"); }
        }

        public object DefaultValue { get; set; }

        public UserInputElement()
        {
            ValueType = typeof(string);
        }

		public object MaxValue { get; set; }

		public object MinValue { get; set; }

        #region ExpressionElement overrides

        public override bool Validate()
        {
            return _inputValue != null && _inputValue.GetType() == ValueType;
        }

        #endregion
    }


}

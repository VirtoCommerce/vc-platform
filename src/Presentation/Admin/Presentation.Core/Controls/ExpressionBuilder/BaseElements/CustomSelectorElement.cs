using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
    public class CustomSelectorElement : UserInputElement
    {
		[NonSerialized]
        public Func<object> ValueSelector;

        public CustomSelectorElement()
        {
            DefaultValue = "select";
        }

        #region ExpressionElement overrides

        public override bool Validate()
        {
            return InputValue != DefaultValue;
        }

        #endregion
    }

}

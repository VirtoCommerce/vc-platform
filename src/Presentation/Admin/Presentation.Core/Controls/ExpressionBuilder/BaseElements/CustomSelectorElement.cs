using System;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class CustomSelectorElement : UserInputElement
    {
        [NonSerialized]
        public Func<object> ValueSelector;

        public CustomSelectorElement()
        {
            DefaultValue = "select".Localize();
        }

        #region ExpressionElement overrides

        public override bool Validate()
        {
            return InputValue != DefaultValue;
        }

        #endregion
    }

}

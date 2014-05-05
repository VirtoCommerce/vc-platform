using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class AndOr : DictionaryElement
    {
        private const string And = " and ",
                            Or = " or ";

		public AndOr()
        {
            AvailableValues = new[] { And, Or };
            DefaultValue = And;
        }

        public bool IsAnd
        {
            get
            {
                return ((string)InputValue) == And;
            }
        }
    }
}

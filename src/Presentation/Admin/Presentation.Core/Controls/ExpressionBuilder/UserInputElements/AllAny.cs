using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class AllAny : DictionaryElement
    {
        private const string All = "all",
                            Any = "any";

        public AllAny()
        {
            AvailableValues = new[] { All, Any };
            DefaultValue = All;
        }

        public bool IsAll
        {
            get
            {
                return ((string)InputValue) == All;
            }
        }
    }
}

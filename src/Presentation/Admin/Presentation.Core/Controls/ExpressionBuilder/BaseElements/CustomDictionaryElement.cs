using System;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[Serializable]
	public class CustomDictionaryElement : UserInputElement
	{
        public object[] AvailableValues { get; set; }

        // rp: new added , this property hides parents DefaultValue and as I get from property usage the alternative virtual for parent is wrong
		public new object[] DefaultValue { get; set; }
	}


}

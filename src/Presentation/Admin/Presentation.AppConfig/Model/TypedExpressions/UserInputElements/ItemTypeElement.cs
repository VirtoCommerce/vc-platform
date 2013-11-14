using System;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.Model
{
    [Serializable]
    public class ItemTypeElement : DictionaryElement
    {
        private string[] Items = Enum.GetNames(typeof(ItemType));

		public ItemTypeElement()
        {
			this.AvailableValues = Items;
            this.DefaultValue = Items[0];
        }
    }
}

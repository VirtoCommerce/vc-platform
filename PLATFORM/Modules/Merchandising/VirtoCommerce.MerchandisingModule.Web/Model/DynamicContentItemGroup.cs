using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class DynamicContentItemGroup
    {
        public DynamicContentItemGroup(string groupName)
        {
            GroupName = groupName;
            Items = new List<DynamicContentItem>();
        }

        /// <summary>
        /// Gets or sets the value of dynamic content item group name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the collection of dynamic content items for dynamic content item group
        /// </summary>
        public List<DynamicContentItem> Items { get; private set; }
    }
}
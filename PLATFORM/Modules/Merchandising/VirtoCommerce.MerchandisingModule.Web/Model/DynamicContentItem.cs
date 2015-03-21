using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class DynamicContentItem
    {
        #region Public Properties

        public string ContentType { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public bool IsMultilingual { get; set; }
        public string Name { get; set; }

        public PropertyDictionary Properties { get; set; }

        #endregion
    }

    public class DynamicContentItemGroup
    {
        #region Constructors and Destructors

        public DynamicContentItemGroup(string groupName)
        {
            this.GroupName = groupName;
            this.Items = new List<DynamicContentItem>();
        }

        #endregion

        #region Public Properties

        public string GroupName { get; set; }
        public List<DynamicContentItem> Items { get; private set; }

        #endregion
    }
}

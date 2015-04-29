#region

using System.Collections.Generic;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts.Contents
{
    public class DynamicContentItemGroup
    {
        #region Constructors and Destructors

        public DynamicContentItemGroup(string groupName)
        {
            GroupName = groupName;
            Items = new List<DynamicContentItem>();
        }

        #endregion

        #region Public Properties

        public string GroupName { get; set; }

        public List<DynamicContentItem> Items { get; private set; }

        #endregion
    }
}

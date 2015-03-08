using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Lists
{
    public class MenuLinkList
    {
        #region Public Properties

        public string Id { get; set; }

        public IEnumerable<MenuLink> MenuLinks { get; set; }
        public string Name { get; set; }

        #endregion
    }
}

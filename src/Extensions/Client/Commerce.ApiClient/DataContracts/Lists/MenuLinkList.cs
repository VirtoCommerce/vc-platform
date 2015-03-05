using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Lists
{
    public class MenuLinkList
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<MenuLink> MenuLinks { get; set; }
    }
}

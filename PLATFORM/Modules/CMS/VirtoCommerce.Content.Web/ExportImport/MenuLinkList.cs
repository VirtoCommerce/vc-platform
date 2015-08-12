using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.ExportImport
{
    public class MenuLinkList
    {
        public MenuLinkList()
        {
            MenuLinks = new List<MenuLink>();
        }

        public string Id { get; set; }
        /// <summary>
        /// Name of menu link list, can be used as title of list in frontend
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Store identifier, for which the list belongs
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// Locale of this menu link list
        /// </summary>
        public string Language { get; set; }

        public ICollection<MenuLink> MenuLinks { get; set; }
    }
}
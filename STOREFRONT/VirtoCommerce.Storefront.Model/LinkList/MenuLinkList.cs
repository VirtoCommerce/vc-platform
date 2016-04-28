using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represents site navigation menu link list object
    /// </summary>
    public class MenuLinkList : Entity, IHasLanguage
    {
        public MenuLinkList()
        {
            MenuLinks = new List<MenuLink>();
        }

        /// <summary>
        /// Gets or sets the name of site navigation menu link list
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the site navigation menu link list store ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the collection of site navigation menu link for link list
        /// </summary>
        public ICollection<MenuLink> MenuLinks { get; set; }

        #region IHasLanguage Members
        /// <summary>
        /// Gets or sets the locale of site navigation menu link list
        /// </summary>
        public Language Language { get; set; }
        #endregion
    }
}
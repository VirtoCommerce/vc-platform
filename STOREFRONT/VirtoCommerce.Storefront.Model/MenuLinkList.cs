using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represents site navigation menu link list object
    /// </summary>
    public class MenuLinkList
    {
        /// <summary>
        /// Gets or sets the ID of site navigation menu link list
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of site navigation menu link list
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the site navigation menu link list store ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the locale of site navigation menu link list
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the collection of site navigation menu link for link list
        /// </summary>
        public ICollection<MenuLink> MenuLinks { get; set; }

        /// <summary>
        /// Gets or sets the collection of security scopes for site navigation menu link list
        /// </summary>
        public ICollection<string> SecurityScopes { get; set; }
    }
}
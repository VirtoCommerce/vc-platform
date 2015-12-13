using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Represents site navigation menu link object
    /// </summary>
    public class MenuLink
    {
        /// <summary>
        /// Gets or sets the ID of site navigation menu link
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title of site navigation menu link
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL of site navigation menu link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the priority of site navigation menu link
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the activity sign of site navigation menu link
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the ID of site navigation menu link list
        /// </summary>
        public string MenuLinkListId { get; set; }

        /// <summary>
        /// Gets or sets the collection of security scopes for site navigation menu link
        /// </summary>
        public ICollection<string> SecurityScopes { get; set; }
    }
}
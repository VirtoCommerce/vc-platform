
namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Editorial review for an item.
    /// </summary>
    public class EditorialReview
    {
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the review content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; set; }
        /// <summary>
        /// Gets or sets the type of the review.
        /// </summary>
        /// <value>
        /// The type of the review.
        /// </value>
        public string ReviewType { get; set; }
        /// <summary>
        /// Gets or sets the review language.
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
        public string LanguageCode { get; set; }

        /// <summary>
        /// System flag used to mark that object was inherited from other
        /// </summary>
        public bool IsInherited { get; set; }
    }
}

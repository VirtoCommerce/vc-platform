
namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Information to define linking information from item or category to category.
    /// </summary>
    public class CategoryLink
    {
        /// <summary>
        /// Gets or sets the source item id.
        /// </summary>
        /// <value>
        /// The source item identifier.
        /// </value>
		public string SourceItemId { get; set; }
        /// <summary>
        /// Gets or sets the source category identifier.
        /// </summary>
        /// <value>
        /// The source category identifier.
        /// </value>
		public string SourceCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the target catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
		public string CatalogId { get; set; }
        /// <summary>
        /// Gets or sets the target category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public string CategoryId { get; set; }
    }
}

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class ItemCategory
    {
        /// <summary>
        /// Gets or sets the value of cataog id
        /// </summary>
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the virtual category
        /// </summary>
        /// <value>
        /// ItemCategory object
        /// </value>
        public ItemCategory VirtualCategories { get; set; }
    }
}
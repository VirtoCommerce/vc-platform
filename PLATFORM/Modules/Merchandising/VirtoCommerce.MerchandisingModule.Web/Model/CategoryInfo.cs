namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class CategoryInfo
    {
        /// <summary>
        /// Gets or sets the value of category info id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value of category info name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of category info SEO parameters
        /// </summary>
        /// <value>
        /// Array of SeoKeyword object
        /// </value>
        public SeoKeyword[] SeoKeywords { get; set; }
    }
}
using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Category
    {
        /// <summary>
        /// Gets or sets the value of category code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the value of category id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value of category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of category parents categories
        /// </summary>
        /// <value>
        /// Collection of Category objects
        /// </value>
        public IEnumerable<Category> Parents { get; set; }

        /// <summary>
        /// Gets or sets the collection of category SEO parameters
        /// </summary>
        /// <value>
        /// Collection of SeoKeyword objects
        /// </value>
        public IEnumerable<SeoKeyword> Seo { get; set; }

        /// <summary>
        /// Gets or sets the category image
        /// </summary>
        /// <value>
        /// Image object
        /// </value>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the flag of virtual category
        /// </summary>
        public bool Virtual { get; set; }
    }
}
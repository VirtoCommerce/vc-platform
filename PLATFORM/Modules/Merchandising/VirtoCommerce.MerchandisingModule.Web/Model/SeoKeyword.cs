namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class SeoKeyword
    {
        /// <summary>
        /// Gets or sets the value of image alternative description
        /// </summary>
        public string ImageAltDescription { get; set; }

        /// <summary>
        /// Gets or sets the value of SEO keyword
        /// </summary>
        /// <value>
        /// URL-slug
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the value of SEO language
        /// </summary>
        /// <value>
        /// Culture name in format languagecode2-country/regioncode2
        /// </value>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the value of webpage meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the value of webpage meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the value of webpage title
        /// </summary>
        public string Title { get; set; }
    }
}
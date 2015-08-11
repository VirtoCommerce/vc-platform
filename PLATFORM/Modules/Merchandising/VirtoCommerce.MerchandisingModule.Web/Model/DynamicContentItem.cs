namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class DynamicContentItem
    {
        /// <summary>
        /// Gets or sets the value of dynamic content item type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the value of dynamic content item description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value of dynamic content item id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the flag of supporting multilanguage for dynamic content item
        /// </summary>
        public bool IsMultilingual { get; set; }

        /// <summary>
        /// Gets or sets the value of dynamic content item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of dynamic content item propertines
        /// </summary>
        public PropertyDictionary Properties { get; set; }
    }
}
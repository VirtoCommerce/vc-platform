namespace VirtoCommerce.Domain.Catalog.Model
{
    public class Facet
    {
        /// <summary>
        /// Gets or sets the value of facet type
        /// </summary>
        /// <value>
        /// "Attribute", "PriceRange", "Range" or "Category"
        /// </value>
        public string FacetType { get; set; }

        /// <summary>
        /// Gets or sets the value of facet field
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the value of facet label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the collection of facet values
        /// </summary>
        /// <value>
        /// Array of FacetValue objects
        /// </value>
        public FacetValue[] Values { get; set; }
    }
}

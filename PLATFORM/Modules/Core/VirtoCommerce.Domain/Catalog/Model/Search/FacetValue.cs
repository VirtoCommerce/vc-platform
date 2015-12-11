namespace VirtoCommerce.Domain.Catalog.Model
{
    public class FacetValue
    {
        /// <summary>
        /// Gets or sets the facet value count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the flag for facet value is applied
        /// </summary>
        public bool IsApplied { get; set; }

        /// <summary>
        /// Gets or sets the facet value label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the facet value
        /// </summary>
        public object Value { get; set; }
    }
}

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class AggregationItem
    {
        /// <summary>
        /// Gets or sets the aggregation item count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the flag for aggregation item is applied
        /// </summary>
        public bool IsApplied { get; set; }

        /// <summary>
        /// Gets or sets the aggregation item label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the aggregation item value
        /// </summary>
        public object Value { get; set; }
    }
}

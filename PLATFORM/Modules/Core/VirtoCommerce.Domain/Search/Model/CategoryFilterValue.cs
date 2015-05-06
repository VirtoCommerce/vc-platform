namespace VirtoCommerce.Domain.Search.Model
{
	public class CategoryFilterValue : ISearchFilterValue
    {
        public string Outline { get; set; }

        /// <summary>
        /// Gets or sets the id. Which contains category code.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        public string Name { get; set; }
    }
}

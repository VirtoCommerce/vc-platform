namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Association
    {
        /// <summary>
        /// Gets or sets the value of catalog item association description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item id for current association
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item association name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the numeric value of catalog item association priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the value of catalog item association type
        /// </summary>
        /// <value>
        /// "CrossSale", "UpSale", "DownSale", "RelatedProducts", "Optional" by default
        /// </value>
        public string Type { get; set; }
    }
}
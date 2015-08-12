namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class EditorialReview
    {
        /// <summary>
        /// Gets or sets the value of editorial review text content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the value of editorial review id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value of editorial review type
        /// </summary>
        /// <value>
        /// "QuickReview" or "FullReview"
        /// </value>
        public string ReviewType { get; set; }
    }
}
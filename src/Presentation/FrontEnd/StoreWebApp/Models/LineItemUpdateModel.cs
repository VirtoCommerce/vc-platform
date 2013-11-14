namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class LineItemUpdateModel.
	/// </summary>
    public class LineItemUpdateModel
    {
		/// <summary>
		/// Gets or sets the line item identifier.
		/// </summary>
		/// <value>The line item identifier.</value>
        public string LineItemId { get; set; }
		/// <summary>
		/// Gets or sets the quantity.
		/// </summary>
		/// <value>The quantity.</value>
        public decimal Quantity { get; set; }
		/// <summary>
		/// Gets or sets the comment.
		/// </summary>
		/// <value>The comment.</value>
        public string Comment { get; set; }
    }
}
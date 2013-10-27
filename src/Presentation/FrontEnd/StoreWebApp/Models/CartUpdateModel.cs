namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CartUpdateModel.
	/// </summary>
    public class CartUpdateModel
    {
		/// <summary>
		/// Gets or sets the line items.
		/// </summary>
		/// <value>The line items.</value>
        public LineItemUpdateModel LineItems { get; set; }
    }
}
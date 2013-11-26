namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CartRemoveModel.
	/// </summary>
    public class CartRemoveModel : CartJsonModel
    {
		/// <summary>
		/// Gets or sets the delete source.
		/// </summary>
		/// <value>The delete source.</value>
		public string DeleteSource { get; set; }
        /// <summary>
        /// Deleted lineItem id
        /// </summary>
        public string DeleteId { get; set; }
    }

	/// <summary>
	/// Class CartJsonModel.
	/// </summary>
	public class CartJsonModel
	{
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }
		/// <summary>
		/// Gets or sets the cart sub total.
		/// </summary>
		/// <value>The cart sub total.</value>
		public string CartSubTotal { get; set; }
		/// <summary>
		/// Gets or sets the cart total.
		/// </summary>
		/// <value>The cart total.</value>
		public string CartTotal { get; set; }
		/// <summary>
		/// Gets or sets the cart count.
		/// </summary>
		/// <value>The cart count.</value>
		public int CartCount { get; set; }
		/// <summary>
		/// Gets or sets the line items view.
		/// </summary>
		/// <value>The line items view.</value>
		public string LineItemsView { get; set; }
        /// <summary>
        /// Cart name
        /// </summary>
        public string CartName { get; set; }
	}
}
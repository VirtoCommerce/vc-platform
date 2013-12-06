namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CartRemoveModel.
	/// </summary>
    public class CartRemoveModel : CartJsonModel
    {
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
	    public CartJsonModel()
	    {
	        Messages = new MessagesModel();
	    }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The delete source.</value>
        public string Source { get; set; }
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public MessagesModel Messages { get; set; }
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
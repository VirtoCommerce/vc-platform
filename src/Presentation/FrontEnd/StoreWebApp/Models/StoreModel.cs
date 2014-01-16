namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class StoreModel.
	/// </summary>
    public class StoreModel
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }
		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
        public string Url { get; set; }
    }

    /// <summary>
    /// Class StoresModel.
    /// </summary>
    public class StoresModel
    {
        /// <summary>
        /// Gets or sets the selected store.
        /// </summary>
        /// <value>The selected store.</value>
        public string SelectedStore { get; set; }

        /// <summary>
        /// Gets or sets the name of the selected store.
        /// </summary>
        /// <value>
        /// The name of the selected store.
        /// </value>
        public string SelectedStoreName { get; set; }
        /// <summary>
        /// Gets or sets the stores.
        /// </summary>
        /// <value>The stores.</value>
        public StoreModel[] Stores { get; set; }
    }
}
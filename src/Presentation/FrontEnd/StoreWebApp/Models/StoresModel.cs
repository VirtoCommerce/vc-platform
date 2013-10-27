namespace VirtoCommerce.Web.Models
{
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
		/// Gets or sets the stores.
		/// </summary>
		/// <value>The stores.</value>
        public StoreModel[] Stores { get; set; }
    }
}
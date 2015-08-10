namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Sync asset element group
	/// </summary>
    public class SyncAssetGroup
    {
		/// <summary>
		/// Asset element group type
		/// </summary>
        public string Type { get; set; }

		/// <summary>
		/// Asset elements
		/// </summary>
        public SyncAsset[] Assets { get; set; }
    }
}
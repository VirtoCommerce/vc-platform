namespace VirtoCommerce.Content.Web.Models
{
    public class SyncAssetGroup
    {
		/// <summary>
		/// Asset element group type, one of the two predefined values - 'pages', 'theme'
		/// </summary>
        public string Type { get; set; }

        public SyncAsset[] Assets { get; set; }
    }
}
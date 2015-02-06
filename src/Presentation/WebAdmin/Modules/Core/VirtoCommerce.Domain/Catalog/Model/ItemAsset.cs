namespace VirtoCommerce.Domain.Catalog.Model
{
	public class ItemAsset
	{
		public string Id { get; set; }
		public string ItemId { get; set; }
		public ItemAssetType Type { get; set; }
		public string Url { get; set; }
	    public string Group { get; set; }
	}
}

using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
	public class ItemAsset : AuditableEntity
	{
		public string ItemId { get; set; }
		public ItemAssetType Type { get; set; }
		public string Url { get; set; }
	    public string Group { get; set; }
	}
}

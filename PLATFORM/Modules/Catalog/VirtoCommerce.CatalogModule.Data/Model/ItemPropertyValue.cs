using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class ItemPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		[StringLength(128)]
		[Required]
		public string ItemId { get; set; }

		[Parent]
		[ForeignKey("ItemId")]
		public virtual Item CatalogItem { get; set; }
		#endregion
	}
}

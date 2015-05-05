using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class CatalogPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		[StringLength(128)]
		[Required]
		public string CatalogId { get; set; }

		[Parent]
		[ForeignKey("CatalogId")]
		public virtual Catalog Catalog { get; set; }
		#endregion
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class CategoryPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		[StringLength(128)]
		[Required]
		public string CategoryId { get; set; }

		[Parent]
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }
		#endregion
	}
}

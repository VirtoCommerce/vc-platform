using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Data.Infrastructure;


namespace VirtoCommerce.CatalogModule.Data.Model
{

	public class PropertyValue : PropertyValueBase
	{
		#region Navigation Properties

		[StringLength(128)]
		public string PropertyId { get; set; }

		[Parent]
		[ForeignKey("PropertyId")]
		public virtual Property Property { get; set; }

		[StringLength(128)]
		[ForeignKey("ParentPropertyValue")]
		public string ParentPropertyValueId { get; set; }

		public virtual PropertyValue ParentPropertyValue { get; set; }

		#endregion
	}
}

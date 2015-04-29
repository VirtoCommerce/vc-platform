using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentPlace : AuditableEntity
	{
		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		[StringLength(2048)]
		public string ImageUrl { get; set; }

		#region Navigation Properties
		[StringLength(128)]
		[ForeignKey("Folder")]
		public string FolderId { get; set; }

		public virtual DynamicContentFolder Folder { get; set; }
		#endregion
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentItem : AuditableEntity
	{
		public DynamicContentItem()
		{
			PropertyValues = new ObservableCollection<DynamicContentItemProperty>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		/// <summary>
		/// available values in DynamicContentType enum
		/// </summary>
		[StringLength(64)]
		public string ContentTypeId { get; set; }

		public bool IsMultilingual { get; set; }

		#region Navigation Properties

		[StringLength(128)]
		[ForeignKey("Folder")]
		public string FolderId { get; set; }

		[StringLength(2048)]
		public string ImageUrl { get; set; }

		public virtual DynamicContentFolder Folder { get; set; }

		public virtual ObservableCollection<DynamicContentItemProperty> PropertyValues { get; set; }

		#endregion
	}
}

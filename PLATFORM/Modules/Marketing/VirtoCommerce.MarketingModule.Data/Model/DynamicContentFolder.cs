using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentFolder : AuditableEntity
	{
		public DynamicContentFolder()
		{
			ContentItems = new ObservableCollection<DynamicContentItem>();
			ContentPlaces = new ObservableCollection<DynamicContentFolder>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(256)]
		public string Description { get; set; }

		[StringLength(2048)]
		public string ImageUrl { get; set; }

		#region Navigation Properties
		[StringLength(128)]
		[ForeignKey("ParentFolder")]
		public string ParentFolderId { get; set; }

		public virtual DynamicContentFolder ParentFolder { get; set; }

		public virtual ObservableCollection<DynamicContentItem> ContentItems { get; set; }

		public virtual ObservableCollection<DynamicContentFolder> ContentPlaces { get; set; }
		#endregion
	}
}

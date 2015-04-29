using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class AssociationGroup : AuditableEntity
	{
		public AssociationGroup()
		{
			Associations = new ObservableCollection<Association>();
		}

		[StringLength(128)]
		[Required]
		public string Name { get; set; }

		[StringLength(512)]
		public string Description { get; set; }

		public int Priority { get; set; }

		#region Navigation Properties

		public string ItemId { get; set; }
		[Parent]
		[ForeignKey("ItemId")]
		public virtual Item CatalogItem { get; set; }

		public virtual ObservableCollection<Association> Associations { get; set; }
		#endregion
	}
}

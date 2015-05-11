using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public abstract class CatalogBase : AuditableEntity
	{
		public CatalogBase()
		{
			Id = Guid.NewGuid().ToString("N");
			CategoryBases = new NullCollection<CategoryBase>();
			IncommingLinks = new NullCollection<CategoryRelation>();
		}

		[Required]
		[StringLength(128)]
		public string Name { get; set; }

		[StringLength(64)]
		[Required]
		public string DefaultLanguage { get; set; }

		[StringLength(128)]
		public string OwnerId { get; set; }

		#region Navigation Properties
		public virtual ObservableCollection<CategoryBase> CategoryBases { get; set; }
		public virtual ObservableCollection<CategoryRelation> IncommingLinks{ get; set; }

		#endregion

	}
}

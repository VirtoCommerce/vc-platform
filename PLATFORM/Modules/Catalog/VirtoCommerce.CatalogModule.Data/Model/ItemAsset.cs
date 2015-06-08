using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class ItemAsset : AuditableEntity
    {
		public ItemAsset()
		{
			Id = Guid.NewGuid().ToString("N");
		}
		[StringLength(2083)]
		[Required]
		public string AssetId { get; set; }

		[StringLength(64)]
		[Required]
		public string AssetType { get; set; }

		[StringLength(64)]
		[Required]
		public string GroupName { get; set; }

		public int SortOrder { get; set; }


		#region Navigation Properties

		public string ItemId { get; set; }
		public virtual Item CatalogItem { get; set; }
		#endregion
    }
}

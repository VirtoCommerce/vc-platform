using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public class LinkedCategory : CategoryBase
    {
		#region Navigation Properties
		[StringLength(128)]
		[ForeignKey("CatalogLink")]
		[Required]
		public string LinkedCatalogId { get; set; }

		public virtual CatalogBase CatalogLink { get; set; }

		[StringLength(128)]
		[ForeignKey("CategoryLink")]
		public string LinkedCategoryId { get; set; }

		public virtual CategoryBase CategoryLink { get; set; }
		#endregion

    }
}

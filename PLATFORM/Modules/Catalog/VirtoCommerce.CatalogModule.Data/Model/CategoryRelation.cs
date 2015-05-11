using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class CategoryRelation : Entity
	{
		public CategoryRelation()
		{
			Id = Guid.NewGuid().ToString("N");
		}

	
		#region Navigation Properties
		[StringLength(128)]
		[Required]
		public string SourceCategoryId { get; set; }
		public virtual Category SourceCategory { get; set; }

		[StringLength(128)]
		public string TargetCatalogId { get; set; }
		public virtual CatalogBase TargetCatalog { get; set; }


		[StringLength(128)]
		public string TargetCategoryId { get; set; }
		public virtual Category TargetCategory { get; set; }
		#endregion
	}
}

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
		#region Navigation Properties
		public string SourceCategoryId { get; set; }
		public virtual Category SourceCategory { get; set; }

		public string TargetCatalogId { get; set; }
		public virtual Catalog TargetCatalog { get; set; }

		public string TargetCategoryId { get; set; }
		public virtual Category TargetCategory { get; set; }
		#endregion
	}
}

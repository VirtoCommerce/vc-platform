using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CatalogModule.Data.Model
{
	public class CategoryPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		public string CategoryId { get; set; }

		public virtual Category Category { get; set; }
		#endregion
	}
}

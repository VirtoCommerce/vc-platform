using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("CatalogPropertyValues")]
	public class CatalogPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		private string _CatalogId;
		[DataMember]
		[StringLength(128)]
		[Required]
		public string CatalogId
		{
			get
			{
				return _CatalogId;
			}
			set
			{
				SetValue(ref _CatalogId, () => this.CatalogId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("CatalogId")]
		public virtual Catalog Catalog { get; set; }
		#endregion
	}
}

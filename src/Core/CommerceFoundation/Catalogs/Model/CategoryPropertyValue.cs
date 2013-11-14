using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("CategoryPropertyValues")]
	public class CategoryPropertyValue : PropertyValueBase
	{
		#region Navigation Properties
		private string _CategoryId;
		[DataMember]
		[StringLength(128)]
		[Required]
		public string CategoryId
		{
			get
			{
				return _CategoryId;
			}
			set
			{
				SetValue(ref _CategoryId, () => this.CategoryId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }
		#endregion
	}
}

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	public class CatalogPromotion : Promotion
	{
		private string _CatalogId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
	
	}
}

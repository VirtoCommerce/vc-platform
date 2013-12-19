using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[EntitySet("DynamicContentItemProperties")]
	public class DynamicContentItemProperty : PropertyValueBase
	{
		#region Navigation Properties

		private string _itemId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[ForeignKey("DynamicContentItem")]
		[Required]
		public string DynamicContentItemId
		{
			get { return _itemId; }
			set { SetValue(ref _itemId, () => DynamicContentItemId, value); }
		}

		[DataMember]
		public virtual DynamicContentItem DynamicContentItem { get; set; }

		#endregion
	}
}

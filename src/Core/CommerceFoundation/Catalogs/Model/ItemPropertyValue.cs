using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Catalogs.Model
{
	[DataContract]
	[EntitySet("ItemPropertyValues")]
	public class ItemPropertyValue : PropertyValueBase
	{
		#region Navigation Properties

		private string _ItemId;
		[DataMember]
		[StringLength(128)]
		[Required]
		public string ItemId
		{
			get
			{
				return _ItemId;
			}
			set
			{
				SetValue(ref _ItemId, () => this.ItemId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("ItemId")]
		public virtual Item CatalogItem { get; set; }
		#endregion

		public override string ToString()
		{
			switch (ValueType)
			{
				case (int)PropertyValueType.Boolean:
					return BooleanValue.ToString();
				case (int)PropertyValueType.DateTime:
					return DateTimeValue.ToString();
				case (int)PropertyValueType.Decimal:
					return DecimalValue.ToString();
				case (int)PropertyValueType.Integer:
					return IntegerValue.ToString();
				case (int)PropertyValueType.LongString:
					return LongTextValue;
				case (int)PropertyValueType.ShortString:
				case (int)PropertyValueType.Image:
					return ShortTextValue;
			}
			return base.ToString();
		}
	}
}

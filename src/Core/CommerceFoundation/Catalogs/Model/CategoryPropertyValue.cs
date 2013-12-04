using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
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

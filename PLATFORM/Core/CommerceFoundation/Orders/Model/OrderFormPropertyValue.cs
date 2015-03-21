using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[EntitySet("OrderFormPropertyValues")]
	public class OrderFormPropertyValue : PropertyValueBase
	{
		#region Navigation Properties

		private string _OrderFormId;
		[DataMember]
		[StringLength(128)]
		public string OrderFormId
		{
			get
			{
				return _OrderFormId;
			}
			set
			{
				SetValue(ref _OrderFormId, () => OrderFormId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("OrderFormId")]
		public virtual OrderForm OrderForm { get; set; }

		#endregion

		public override string ToString()
		{
			switch (ValueType)
			{
				case (int)OrderFormValueType.Boolean:
					return BooleanValue.ToString();
				case (int)OrderFormValueType.DateTime:
					return DateTimeValue.ToString();
				case (int)OrderFormValueType.Decimal:
					return DecimalValue.ToString();
				case (int)OrderFormValueType.Integer:
					return IntegerValue.ToString();
				case (int)OrderFormValueType.LongString:
					return LongTextValue;
				case (int)OrderFormValueType.ShortString:
					return ShortTextValue;
			}
			return base.ToString();
		}
	}
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("CasePropertyValues")]
	public class CasePropertyValue : PropertyValueBase
	{
		private int _Priority;
		[DataMember]
		public int Priority
		{
			get
			{
				return _Priority;
			}
			set
			{
				SetValue(ref _Priority, () => Priority, value);
			}
		}

		#region NavigationProperties

		private string _CaseId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CaseId
		{
			get { return _CaseId; }
			set { SetValue(ref _CaseId, () => CaseId, value); }
		}

		[DataMember]
		[ForeignKey("CaseId")]
		[Parent]
		public virtual Case Case { get; set; }
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
					return ShortTextValue;
			}
			return base.ToString();
		}
	}
}

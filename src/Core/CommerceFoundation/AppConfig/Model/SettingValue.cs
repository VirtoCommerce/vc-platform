using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Globalization;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("SettingValues")]
	[DataServiceKey("SettingValueId")]
	public class SettingValue : StorageEntity
	{
		public const string TypeShortText = "ShortText";
		public const string TypeLongText = "LongText";
		public const string TypeInteger = "Integer";
		public const string TypeDecimal = "Decimal";
		public const string TypeBoolean = "Boolean";
		public const string TypeDateTime = "DateTime";

		public SettingValue()
		{
			_settingValueId = GenerateNewKey();
		}

		private string _settingValueId;

		[Key]
		[StringLength(128)]
		[DataMember]
		public string SettingValueId
		{
			get { return _settingValueId; }
			set { SetValue(ref _settingValueId, () => SettingValueId, value); }
		}

		private string _valueType;
		[Required]
		[DataMember]
		[StringLength(64)]
		public string ValueType
		{
			get
			{
				return _valueType;
			}
			set
			{
				SetValue(ref _valueType, () => ValueType, value);
			}
		}

		private string _shortTextValue;
		[StringLength(512)]
		[DataMember]
		public string ShortTextValue
		{
			get
			{
				return _shortTextValue;
			}
			set
			{
				SetValue(ref _shortTextValue, () => ShortTextValue, value);
			}
		}

		private string _longTextValue;
		[DataMember]
		public string LongTextValue
		{
			get
			{
				return _longTextValue;
			}
			set
			{
				SetValue(ref _longTextValue, () => LongTextValue, value);
			}
		}

		private decimal _decimalValue;
		[DataMember]
		public decimal DecimalValue
		{
			get
			{
				return _decimalValue;
			}
			set
			{
				SetValue(ref _decimalValue, () => DecimalValue, value);
			}
		}

		private int _integerValue;
		[DataMember]
		public int IntegerValue
		{
			get
			{
				return _integerValue;
			}
			set
			{
				SetValue(ref _integerValue, () => IntegerValue, value);
			}
		}

		private bool _booleanValue;
		[DataMember]
		public bool BooleanValue
		{
			get
			{
				return _booleanValue;
			}
			set
			{
				SetValue(ref _booleanValue, () => BooleanValue, value);
			}
		}

		private DateTime? _dateTimeValue;
		[DataMember]
		public DateTime? DateTimeValue
		{
			get
			{
				return _dateTimeValue;
			}
			set
			{
				SetValue(ref _dateTimeValue, () => DateTimeValue, value);
			}
		}

		private string _locale;
		[StringLength(64)]
		[DataMember]
		public string Locale
		{
			get
			{
				return _locale;
			}
			set
			{
				SetValue(ref _locale, () => Locale, value);
			}
		}

		public override string ToString()
		{
			return ToString(CultureInfo.CurrentCulture);
		}

		public string ToString(IFormatProvider formatProvider)
		{
			switch (ValueType)
			{
				case TypeBoolean:
					return BooleanValue.ToString();
				case TypeDateTime:
					return DateTimeValue == null ? null : DateTimeValue.Value.ToString(formatProvider);
				case TypeDecimal:
					return DecimalValue.ToString(formatProvider);
				case TypeInteger:
					return IntegerValue.ToString(formatProvider);
				case TypeLongText:
					return LongTextValue;
				case TypeShortText:
					return ShortTextValue;
				default:
					return base.ToString();
			}
		}

		public object RawValue()
		{
			switch (ValueType)
			{
				case TypeBoolean:
					return BooleanValue;
				case TypeDateTime:
					return DateTimeValue;
				case TypeDecimal:
					return DecimalValue;
				case TypeInteger:
					return IntegerValue;
				case TypeLongText:
					return LongTextValue;
				case TypeShortText:
					return ShortTextValue;
				default:
					return null;
			}
		}

		#region Navigation properties

		private string _settingId;

		[StringLength(128)]
		[Required]
		[DataMember]
		public string SettingId
		{
			get
			{
				return _settingId;
			}
			set
			{
				SetValue(ref _settingId, () => SettingId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("SettingId")]
		public virtual Setting Setting { get; set; }

		#endregion
	}
}

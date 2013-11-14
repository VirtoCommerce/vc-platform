using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	[DataContract]
	[EntitySet("SettingValues")]
	[DataServiceKey("SettingValueId")]
	public class SettingValue: StorageEntity
	{
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

		private string _ValueType;
		[Required]
		[DataMember]
		[StringLength(64)]
		public string ValueType
		{
			get
			{
				return _ValueType;
			}
			set
			{
				SetValue(ref _ValueType, () => this.ValueType, value);
			}
		}

		private string _ShortTextValue;
		[StringLength(512)]
		[DataMember]
		public string ShortTextValue
		{
			get
			{
				return _ShortTextValue;
			}
			set
			{
				SetValue(ref _ShortTextValue, () => this.ShortTextValue, value);
			}
		}

		private string _LongTextValue;
		[DataMember]
		public string LongTextValue
		{
			get
			{
				return _LongTextValue;
			}
			set
			{
				SetValue(ref _LongTextValue, () => this.LongTextValue, value);
			}
		}

		private decimal _DecimalValue;
		[DataMember]
		public decimal DecimalValue
		{
			get
			{
				return _DecimalValue;
			}
			set
			{
				SetValue(ref _DecimalValue, () => this.DecimalValue, value);
			}
		}

		private int _IntegerValue;
		[DataMember]
		public int IntegerValue
		{
			get
			{
				return _IntegerValue;
			}
			set
			{
				SetValue(ref _IntegerValue, () => this.IntegerValue, value);
			}
		}

		private bool _BooleanValue;
		[DataMember]
		public bool BooleanValue
		{
			get
			{
				return _BooleanValue;
			}
			set
			{
				SetValue(ref _BooleanValue, () => this.BooleanValue, value);
			}
		}

		private DateTime? _DateTimeValue;
		[DataMember]
		public DateTime? DateTimeValue
		{
			get
			{
				return _DateTimeValue;
			}
			set
			{
				SetValue(ref _DateTimeValue, () => this.DateTimeValue, value);
			}
		}

		private string _Locale;
		[StringLength(64)]
		[DataMember]
		public string Locale
		{
			get
			{
				return _Locale;
			}
			set
			{
				SetValue(ref _Locale, () => this.Locale, value);
			}
		}

		public override string ToString()
		{
			switch (this.ValueType)
			{
				case textBoolean:
					return this.BooleanValue.ToString();
				case textDateTime:
					return this.DateTimeValue.ToString();
				case textDecimal:
					return this.DecimalValue.ToString();
				case textInteger:
					return this.IntegerValue.ToString();
				case textLongText:
					return this.LongTextValue;
				case textShortText:
					return this.ShortTextValue;
			}
			return base.ToString();
		}

		private const string textShortText = "ShortText",
			textLongText = "LongText",
			textInteger = "Integer",
			textDecimal = "Decimal",
			textBoolean = "Boolean",
			textDateTime = "DateTime";

		#region Navigation properties

		private string _SettingId;

		[StringLength(128)]
		[Required]
		[DataMember]
		public string SettingId
		{
			get
			{
				return _SettingId;
			}
			set
			{
				SetValue(ref _SettingId, () => SettingId, value);
			}
		}

		[DataMember]
		[Parent]
		[ForeignKey("SettingId")]
		public virtual Setting Setting { get; set; }

		#endregion
	}
}

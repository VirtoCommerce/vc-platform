using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[DataServiceKey("PropertyValueId")]
	public abstract class PropertyValueBase : StorageEntity
	{
		public PropertyValueBase()
		{
			_PropertyValueId = GenerateNewKey();
		}

		private string _PropertyValueId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string PropertyValueId
		{
			get
			{
				return _PropertyValueId;
			}
			set
			{
				SetValue(ref _PropertyValueId, () => this.PropertyValueId, value);
			}
		}

		private string _Alias;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Alias
		{
			get
			{
				return _Alias;
			}
			set
			{
				SetValue(ref _Alias, () => this.Alias, value);
			}
		}

		private string _Name;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}

		private string _KeyValue;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string KeyValue
		{
			get
			{
				return _KeyValue;
			}
			set
			{
				SetValue(ref _KeyValue, () => this.KeyValue, value);
			}
		}

		private int _ValueType;
		[Required]
		[DataMember]
		public int ValueType
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
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
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
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
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
				case (int)PropertyValueType.Image:
				case (int)PropertyValueType.Category:
					return LongTextValue;
                case (int)PropertyValueType.ShortString:
					return ShortTextValue;
            }
            return base.ToString();
        }
	}
}

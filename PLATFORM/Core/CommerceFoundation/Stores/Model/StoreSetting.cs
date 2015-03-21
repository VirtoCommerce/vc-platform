using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Stores.Model
{
    [DataContract]
    [EntitySet("StoreSettings")]
    [DataServiceKey("StoreSettingId")]
    public class StoreSetting : StorageEntity
    {
        public StoreSetting()
        {
            _StoreSettingId = GenerateNewKey();
        }

        private string _StoreSettingId;
        [Key]
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreSettingId
        {
            get
            {
                return _StoreSettingId;
            }
            set
            {
                SetValue(ref _StoreSettingId, () => StoreSettingId, value);
            }
        }

        private string _Name;
        [Required(ErrorMessage = "Field 'Name' is required.")]
        [StringLength(64)]
        [DataMember]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => Name, value);
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
                SetValue(ref _ShortTextValue, () => ShortTextValue, value);
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
                SetValue(ref _LongTextValue, () => LongTextValue, value);
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
                SetValue(ref _DecimalValue, () => DecimalValue, value);
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
                SetValue(ref _IntegerValue, () => IntegerValue, value);
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
                SetValue(ref _BooleanValue, () => BooleanValue, value);
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
                SetValue(ref _DateTimeValue, () => DateTimeValue, value);
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
                SetValue(ref _Locale, () => Locale, value);
            }
        }

        private string _ValueType;
        [DataMember]
        [Required(ErrorMessage = "Field 'Type' is required.")]
        [StringLength(64)]
        public string ValueType
        {
            get
            {
                return _ValueType;
            }
            set
            {
                SetValue(ref _ValueType, () => ValueType, value);
            }
        }

		public override string ToString()
		{
			switch (ValueType)
			{
				case "Boolean":
					return BooleanValue.ToString();
				case "DateTime":
					return DateTimeValue.ToString();
				case "Decimal":
					return DecimalValue.ToString();
				case "Integer":
					return IntegerValue.ToString();
				case "LongText":
                case "xml":
					return LongTextValue;
				case "ShortText":
					return ShortTextValue;
			}
			return base.ToString();
		}

        public object RawValue()
        {
            switch (ValueType)
            {
                case "Boolean":
                    return BooleanValue;
                case "DateTime":
                    return DateTimeValue;
                case "Decimal":
                    return DecimalValue;
                case "Integer":
                    return IntegerValue;
                case "LongText":
                    return LongTextValue;
                case "ShortText":
                    return ShortTextValue;
            }

            return null;
        }

        #region Navigation Properties

        private string _StoreId;
        [ForeignKey("Store")]
        [DataMember]
        [Required]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string StoreId
        {
            get
            {
                return _StoreId;
            }
            set
            {
                SetValue(ref _StoreId, () => StoreId, value);
            }
        }

        [DataMember]
        public Store Store { get; set; }

        #endregion
    }
}

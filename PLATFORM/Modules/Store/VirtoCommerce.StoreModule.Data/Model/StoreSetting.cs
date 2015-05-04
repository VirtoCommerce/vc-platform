using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.StoreModule.Data.Model
{
    public class StoreSetting : Entity
    {
		[Required]
		[StringLength(64)]
		public string Name { get; set; }

        [StringLength(512)]
		public string ShortTextValue { get; set; }

		public string LongTextValue { get; set; }

		public decimal DecimalValue { get; set; }

		public int IntegerValue { get; set; }

		public bool BooleanValue { get; set; }

		public DateTime? DateTimeValue { get; set; }

        [StringLength(64)]
		public string Locale { get; set; }

        [Required]
        [StringLength(64)]
		public string ValueType { get; set; }

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
				case "xml":
                    return LongTextValue;
                case "ShortText":
                    return ShortTextValue;
            }

            return null;
        }

        #region Navigation Properties

		[ForeignKey("Store")]
		[Required]
		[StringLength(128)]
		public string StoreId { get; set; }

        public Store Store { get; set; }

        #endregion
    }
}

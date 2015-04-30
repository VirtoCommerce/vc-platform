using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Model
{
    public abstract class PropertyValueBase : AuditableEntity
    {
		public PropertyValueBase()
		{
			Id = Guid.NewGuid().ToString("N");
		}
		[StringLength(64)]
		public string Alias { get; set; }

		[StringLength(64)]
		public string Name { get; set; }

        [StringLength(128)]
		public string KeyValue { get; set; }

        [Required]
		public int ValueType { get; set; }

        [StringLength(512)]
		public string ShortTextValue { get; set; }

		public string LongTextValue { get; set; }

		public decimal DecimalValue { get; set; }

		public int IntegerValue { get; set; }

		public bool BooleanValue { get; set; }

		public DateTime? DateTimeValue { get; set; }

        [StringLength(64)]
		public string Locale { get; set; }

        public override string ToString()
        {
            switch (ValueType)
            {
                case (int)PropertyValueType.Boolean:
                    return BooleanValue.ToString();
                case (int)PropertyValueType.DateTime:
                    return DateTimeValue.Value.ToString("d");
                case (int)PropertyValueType.Number:
					return DecimalValue.ToString();
                case (int)PropertyValueType.LongText:
                    return LongTextValue;
				default:
                    return ShortTextValue;
            }
        }

        public object ToObjectValue()
        {
            switch (ValueType)
            {
                case (int) PropertyValueType.Boolean:
                    return BooleanValue;
                case (int) PropertyValueType.DateTime:
                    return DateTimeValue;
                case (int) PropertyValueType.Number:
					return DecimalValue;
                case (int) PropertyValueType.LongText:
                    return LongTextValue;
                default:
                    return ShortTextValue;
            }
        }
    }
}

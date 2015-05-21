using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class OperationPropertyEntity : Entity
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
			if (ValueType == null)
				return base.ToString();

			switch (ValueType.ToLowerInvariant())
			{
				case "boolean":
					return BooleanValue.ToString();
				case "datetime":
					return DateTimeValue.ToString();
				case "decimal":
					return DecimalValue.ToString();
				case "integer":
					return IntegerValue.ToString();
				case "longtext":
				case "xml":
					return LongTextValue;
				case "shorttext":
					return ShortTextValue;
			}
			return base.ToString();
		}

		public object RawValue()
		{
			if (ValueType == null)
				return null;

			switch (ValueType.ToLowerInvariant())
			{
				case "boolean":
					return BooleanValue;
				case "datetime":
					return DateTimeValue;
				case "decimal":
					return DecimalValue;
				case "integer":
					return IntegerValue;
				case "longtext":
				case "xml":
					return LongTextValue;
				case "shorttext":
					return ShortTextValue;
			}

			return null;
		}

		#region Navigation Properties

		[Required]
		[StringLength(128)]
		public string OperationId { get; set; }

		public OperationEntity Operation { get; set; }

		#endregion
	}
}

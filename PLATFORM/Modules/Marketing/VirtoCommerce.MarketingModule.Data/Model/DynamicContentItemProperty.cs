using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
	public class DynamicContentItemProperty : AuditableEntity
	{
		[StringLength(64)]
		public string Alias { get; set; }

		[StringLength(128)]
		public string Name { get; set;}

		[StringLength(128)]
		public string KeyValue { get; set;}

		[Required]
		public int ValueType { get; set;} 

		[StringLength(512)]
		public string ShortTextValue { get; set;} 

		public string LongTextValue { get; set;} 

		public decimal DecimalValue { get; set;} 

		public int IntegerValue { get; set;} 

		public bool BooleanValue { get; set;} 

		public DateTime? DateTimeValue { get; set;} 

		[StringLength(64)]
		public string Locale { get; set;} 


		#region Navigation Properties

		[StringLength(128)]
		[ForeignKey("DynamicContentItem")]
		[Required]
		public string DynamicContentItemId { get; set; }  
		public virtual DynamicContentItem DynamicContentItem { get; set; }

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
				case (int)PropertyValueType.LongText:
					return LongTextValue;
				case (int)PropertyValueType.ShortText:
					return ShortTextValue;
			}
			return base.ToString();
		}
	}
}

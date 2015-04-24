using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Model
{
    public class ContactPropertyValue : Entity
    {
		[StringLength(128)]
		public string Name { get; set; }

		public int ValueType { get; set; }

		[StringLength(512)]
		public string ShortTextValue { get; set; }

		public string LongTextValue { get; set; }

		public decimal DecimalValue { get; set; }

		public int IntegerValue { get; set; }

		public bool BooleanValue { get; set; }

		public DateTime? DateTimeValue { get; set; }

		public int Priority { get; set; }

        #region NavigationProperties

  		[StringLength(128)]
		public string ContactId { get; set; }

        [ForeignKey("ContactId")]
        [Parent]
        public virtual Contact Contact { get; set; }

        #endregion
    }
}

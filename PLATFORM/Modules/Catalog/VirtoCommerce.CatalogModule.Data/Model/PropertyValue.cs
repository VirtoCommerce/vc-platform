using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;


namespace VirtoCommerce.CatalogModule.Data.Model
{

	public class PropertyValue : AuditableEntity
    {
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


        #region Navigation Properties
        public string ItemId { get; set; }
        public virtual Item CatalogItem { get; set; }

        public string CatalogId { get; set; }
        public virtual Catalog Catalog { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        #endregion
	}
}

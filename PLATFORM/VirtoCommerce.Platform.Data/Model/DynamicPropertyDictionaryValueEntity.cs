using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyDictionaryValueEntity : AuditableEntity
    {
        public string DictionaryItemId { get; set; }
        public virtual DynamicPropertyDictionaryItemEntity DictionaryItem { get; set; }

        [StringLength(64)]
        public string Locale { get; set; }

        [StringLength(512)]
        public string Value { get; set; }
    }
}

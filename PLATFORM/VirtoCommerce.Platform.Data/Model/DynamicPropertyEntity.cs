using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyEntity : AuditableEntity
    {
        public DynamicPropertyEntity()
        {
            DisplayNames = new NullCollection<DynamicPropertyNameEntity>();
            DictionaryItems = new NullCollection<DynamicPropertyDictionaryItemEntity>();
            ObjectValues = new NullCollection<DynamicPropertyObjectValueEntity>();
        }

        [StringLength(256)]
        public string ObjectType { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(64)]
        public string ValueType { get; set; }

        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsMultilingual { get; set; }
        public bool IsRequired { get; set; }

        public virtual ObservableCollection<DynamicPropertyNameEntity> DisplayNames { get; set; }
        public virtual ObservableCollection<DynamicPropertyDictionaryItemEntity> DictionaryItems { get; set; }
        public virtual ObservableCollection<DynamicPropertyObjectValueEntity> ObjectValues { get; set; }
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyDictionaryItemEntity : AuditableEntity
    {
        public DynamicPropertyDictionaryItemEntity()
        {
            DictionaryValues = new NullCollection<DynamicPropertyDictionaryValueEntity>();
            ObjectValues = new NullCollection<DynamicPropertyObjectValueEntity>();
        }

        public string PropertyId { get; set; }
        public virtual DynamicPropertyEntity Property { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public virtual ObservableCollection<DynamicPropertyDictionaryValueEntity> DictionaryValues { get; set; }
        public virtual ObservableCollection<DynamicPropertyObjectValueEntity> ObjectValues { get; set; }
    }
}

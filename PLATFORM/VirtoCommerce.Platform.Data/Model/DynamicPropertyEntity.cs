using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyEntity : AuditableEntity
    {
        public DynamicPropertyEntity()
        {
            Names = new NullCollection<DynamicPropertyNameEntity>();
            Values = new NullCollection<DynamicPropertyValueEntity>();
        }

        [StringLength(256)]
        public string ObjectType { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(64)]
        public string ValueType { get; set; }

        public bool IsArray { get; set; }

        public virtual ObservableCollection<DynamicPropertyNameEntity> Names { get; set; }
        public virtual ObservableCollection<DynamicPropertyValueEntity> Values { get; set; }
    }
}

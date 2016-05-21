using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyNameEntity : AuditableEntity
    {
        public string PropertyId { get; set; }
        public virtual DynamicPropertyEntity Property { get; set; }

        [StringLength(64)]
        public string Locale { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
    }
}

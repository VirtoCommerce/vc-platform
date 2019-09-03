using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyNameEntity : AuditableEntity
    {
        [StringLength(64)]
        public string Locale { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public string PropertyId { get; set; }
        public virtual DynamicPropertyEntity Property { get; set; }

        public virtual DynamicPropertyName ToModel(DynamicPropertyName propName)
        {
            if (propName == null)
            {
                throw new ArgumentNullException(nameof(propName));
            }

            propName.Locale = Locale;
            propName.Name = Name;
            propName.Description = Description;

            return propName;
        }

        public virtual DynamicPropertyNameEntity FromModel(DynamicPropertyName propName)
        {
            if (propName == null)
            {
                throw new ArgumentNullException(nameof(propName));
            }

            Locale = propName.Locale;
            Name = propName.Name;
            Description = propName.Description;

            return this;
        }

        public virtual void Patch(DynamicPropertyNameEntity target)
        {
            //Nothing to do
        }
    }
}

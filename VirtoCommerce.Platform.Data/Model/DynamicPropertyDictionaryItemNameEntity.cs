using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyDictionaryItemNameEntity : AuditableEntity
    {
        public string DictionaryItemId { get; set; }
        public virtual DynamicPropertyDictionaryItemEntity DictionaryItem { get; set; }

        [StringLength(64)]
        public string Locale { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public virtual DynamicPropertyDictionaryItemName ToModel(DynamicPropertyDictionaryItemName itemName)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }

            itemName.Locale = Locale;
            itemName.Name = Name;

            return itemName;
        }

        public virtual DynamicPropertyDictionaryItemNameEntity FromModel(DynamicPropertyDictionaryItemName itemName)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }

            Locale = itemName.Locale;
            Name = itemName.Name;

            return this;
        }

        public virtual void Patch(DynamicPropertyDictionaryItemNameEntity target)
        {
            //Nothing to do
        }
    }
}

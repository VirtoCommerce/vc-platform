using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Model
{
    public class DynamicPropertyDictionaryItemEntity : AuditableEntity
    {
        public DynamicPropertyDictionaryItemEntity()
        {
            DisplayNames = new NullCollection<DynamicPropertyDictionaryItemNameEntity>();
        }

        public string PropertyId { get; set; }
        public virtual DynamicPropertyEntity Property { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public virtual ObservableCollection<DynamicPropertyDictionaryItemNameEntity> DisplayNames { get; set; }

        public virtual DynamicPropertyDictionaryItem ToModel(DynamicPropertyDictionaryItem dictItem)
        {
            if (dictItem == null)
            {
                throw new ArgumentNullException(nameof(dictItem));
            }

            dictItem.Id = Id;
            dictItem.PropertyId = PropertyId;
            dictItem.Name = Name;
            dictItem.DisplayNames = DisplayNames.Select(x => x.ToModel(AbstractTypeFactory<DynamicPropertyDictionaryItemName>.TryCreateInstance())).ToArray();
            return dictItem;
        }

        public virtual DynamicPropertyDictionaryItemEntity FromModel(DynamicPropertyDictionaryItem dictItem)
        {
            if (dictItem == null)
            {
                throw new ArgumentNullException(nameof(dictItem));
            }
            Id = dictItem.Id;
            PropertyId = dictItem.PropertyId;
            Name = dictItem.Name;
            if (dictItem.DisplayNames != null)
            {
                DisplayNames = new ObservableCollection<DynamicPropertyDictionaryItemNameEntity>(dictItem.DisplayNames.Select(x => AbstractTypeFactory<DynamicPropertyDictionaryItemNameEntity>.TryCreateInstance().FromModel(x)));
            }
            return this;
        }

        public virtual void Patch(DynamicPropertyDictionaryItemEntity target)
        {
            target.Name = Name;
            if (!DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyDictionaryItemNameEntity v) => string.Join("-", v.Locale, v.Name));
                DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}

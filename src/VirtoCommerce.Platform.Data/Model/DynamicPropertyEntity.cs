using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Model
{

    public class DynamicPropertyEntity : AuditableEntity
    {
        public DynamicPropertyEntity()
        {
            DisplayNames = new NullCollection<DynamicPropertyNameEntity>();
            DictionaryItems = new NullCollection<DynamicPropertyDictionaryItemEntity>();
            //ObjectValues = new NullCollection<DynamicPropertyObjectValueEntity>();
        }

        [StringLength(256)]
        public string ObjectType { get; set; }

        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [StringLength(64)]
        public string ValueType { get; set; }

        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsMultilingual { get; set; }
        public bool IsRequired { get; set; }
        public int? DisplayOrder { get; set; }

        public virtual ObservableCollection<DynamicPropertyNameEntity> DisplayNames { get; set; }
        public virtual ObservableCollection<DynamicPropertyDictionaryItemEntity> DictionaryItems { get; set; }
        //public virtual ObservableCollection<DynamicPropertyObjectValueEntity> ObjectValues { get; set; }


        public virtual DynamicProperty ToModel(DynamicProperty dynamicProp)
        {
            if (dynamicProp == null)
            {
                throw new ArgumentNullException(nameof(dynamicProp));
            }

            dynamicProp.Id = Id;
            dynamicProp.CreatedBy = CreatedBy;
            dynamicProp.CreatedDate = CreatedDate;
            dynamicProp.ModifiedBy = ModifiedBy;
            dynamicProp.ModifiedDate = ModifiedDate;
            dynamicProp.Description = Description;
            dynamicProp.DisplayOrder = DisplayOrder;
            dynamicProp.IsArray = IsArray;
            dynamicProp.IsDictionary = IsDictionary;
            dynamicProp.IsMultilingual = IsMultilingual;
            dynamicProp.IsRequired = IsRequired;
            dynamicProp.Name = Name;
            dynamicProp.ObjectType = ObjectType;

            dynamicProp.ValueType = EnumUtility.SafeParse(ValueType, DynamicPropertyValueType.LongText);
            dynamicProp.DisplayNames = DisplayNames.Select(x => x.ToModel(AbstractTypeFactory<DynamicPropertyName>.TryCreateInstance())).ToArray();
            //if (dynamicProp is DynamicObjectProperty dynamicObjectProp)
            //{
            //    dynamicObjectProp.Values = ObjectValues.Select(x => x.ToModel(AbstractTypeFactory<DynamicPropertyObjectValue>.TryCreateInstance())).ToArray();
            //}
            return dynamicProp;
        }

        public virtual DynamicPropertyEntity FromModel(DynamicProperty dynamicProp, PrimaryKeyResolvingMap pkMap)
        {
            if (dynamicProp == null)
            {
                throw new ArgumentNullException(nameof(dynamicProp));
            }

            pkMap.AddPair(dynamicProp, this);

            Id = dynamicProp.Id;
            CreatedBy = dynamicProp.CreatedBy;
            CreatedDate = dynamicProp.CreatedDate;
            ModifiedBy = dynamicProp.ModifiedBy;
            ModifiedDate = dynamicProp.ModifiedDate;
            Description = dynamicProp.Description;
            DisplayOrder = dynamicProp.DisplayOrder;
            IsArray = dynamicProp.IsArray;
            IsDictionary = dynamicProp.IsDictionary;
            IsMultilingual = dynamicProp.IsMultilingual;
            IsRequired = dynamicProp.IsRequired;
            Name = dynamicProp.Name;
            ObjectType = dynamicProp.ObjectType;

            ValueType = dynamicProp.ValueType.ToString();
            if (dynamicProp.DisplayNames != null)
            {
                DisplayNames = new ObservableCollection<DynamicPropertyNameEntity>(dynamicProp.DisplayNames.Select(x => AbstractTypeFactory<DynamicPropertyNameEntity>.TryCreateInstance().FromModel(x)));
            }

            if (dynamicProp is DynamicObjectProperty dynamicObjectProp && dynamicObjectProp.Values != null)
            {
                //Force set these properties from owned property object
                foreach (var value in dynamicObjectProp.Values)
                {
                    value.ObjectId = dynamicObjectProp.ObjectId;
                    value.ObjectType = dynamicObjectProp.ObjectType;
                    value.ValueType = dynamicObjectProp.ValueType;
                }
                //ObjectValues = new ObservableCollection<DynamicPropertyObjectValueEntity>(dynamicObjectProp.Values.Select(x => AbstractTypeFactory<DynamicPropertyObjectValueEntity>.TryCreateInstance().FromModel(x)));
            }
            return this;
        }

        public virtual void Patch(DynamicPropertyEntity target)
        {
            target.Name = Name;
            target.Description = Description;
            target.IsRequired = IsRequired;
            target.IsArray = IsArray;
            target.DisplayOrder = DisplayOrder;

            if (!DisplayNames.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((DynamicPropertyNameEntity x) => string.Join("-", x.Locale, x.Name));
                DisplayNames.Patch(target.DisplayNames, comparer, (sourceItem, targetItem) => { });
            }

            //if (!ObjectValues.IsNullCollection())
            //{
            //    var comparer = AnonymousComparer.Create((DynamicPropertyObjectValueEntity x) => $"{x.ObjectId}:{x.ObjectType}:{x.Locale}:{x.GetValue(EnumUtility.SafeParse(x.ValueType, DynamicPropertyValueType.LongText))}");
            //    ObjectValues.Patch(target.ObjectValues, comparer, (sourceValue, targetValue) => sourceValue.Patch(targetValue));
            //}
        }
    }
}

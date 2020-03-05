using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Model
{
    public class SettingEntity : AuditableEntity
    {
        public SettingEntity()
        {
            SettingValues = new NullCollection<SettingValueEntity>();
        }

        [StringLength(128)]
        public string ObjectType { get; set; }

        [StringLength(128)]
        public string ObjectId { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public virtual ObservableCollection<SettingValueEntity> SettingValues { get; set; }


        public virtual ObjectSettingEntry ToModel(ObjectSettingEntry objSetting)
        {
            if (objSetting == null)
            {
                throw new ArgumentNullException(nameof(objSetting));
            }
            // Don't set name to avoid overwriting with db-readed value (it can be misswritten)
            objSetting.ObjectType = ObjectType;
            objSetting.ObjectId = ObjectId;
            var values = SettingValues.Select(x => x.GetValue()).ToArray();

            if (objSetting.IsDictionary)
            {
                objSetting.AllowedValues = values;
            }
            else
            {
                objSetting.Value = values.FirstOrDefault();
            }

            return objSetting;
        }

        public virtual SettingEntity FromModel(ObjectSettingEntry objectSettingEntry)
        {
            if (objectSettingEntry == null)
            {
                throw new ArgumentNullException(nameof(objectSettingEntry));
            }
            ObjectType = objectSettingEntry.ObjectType;
            ObjectId = objectSettingEntry.ObjectId;
            Name = objectSettingEntry.Name;
            if (objectSettingEntry.IsDictionary)
            {
                SettingValues = new ObservableCollection<SettingValueEntity>(objectSettingEntry.AllowedValues.Select(x => new SettingValueEntity { }.SetValue(objectSettingEntry.ValueType, x)));
            }
            else
            {
                SettingValues = new ObservableCollection<SettingValueEntity>(new[] { new SettingValueEntity { }.SetValue(objectSettingEntry.ValueType, objectSettingEntry.Value) });
            }
            return this;
        }

        public virtual void Patch(SettingEntity target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (!SettingValues.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((SettingValueEntity x) => x.ToString(EnumUtility.SafeParse(x.ValueType, Core.Settings.SettingValueType.LongText), CultureInfo.InvariantCulture) ?? string.Empty);
                SettingValues.Patch(target.SettingValues, comparer, (sourceSetting, targetSetting) => { });
            }
        }
    }
}

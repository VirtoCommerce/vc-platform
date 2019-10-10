using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyDictionaryItem : AuditableEntity, ICloneable
    {
        public string PropertyId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Item names for different languages.
        /// </summary>
        public DynamicPropertyDictionaryItemName[] DisplayNames { get; set; }

        #region ICloneable members

        public virtual object Clone()
        {
            var result = MemberwiseClone() as DynamicPropertyDictionaryItem;

            if (DisplayNames != null)
            {
                result.DisplayNames = DisplayNames.Select(x => x.Clone() as DynamicPropertyDictionaryItemName).ToArray();
            }

            return result;
        }

        #endregion
    }
}

using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings
{
    public interface IHasSettings : IEntity
    {
        string TypeName { get; }
        ICollection<ObjectSettingEntry> Settings { get; set; }
    }
}

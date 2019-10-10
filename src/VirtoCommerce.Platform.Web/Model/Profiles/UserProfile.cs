using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Model.Profiles
{
    public class UserProfile : Entity, IHasSettings
    {
        public virtual ICollection<ObjectSettingEntry> Settings { get; set; } = new List<ObjectSettingEntry>();
        public virtual string TypeName => GetType().Name;
    }
}

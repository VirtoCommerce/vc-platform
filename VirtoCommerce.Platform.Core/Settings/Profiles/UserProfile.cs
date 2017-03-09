using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings.Profiles
{
    public class UserProfile : Entity, IHaveSettings
    {
        public UserProfile(string userId)
        {
            Id = userId;
        }

        public ICollection<SettingEntry> Settings { get; set; }
    }
}

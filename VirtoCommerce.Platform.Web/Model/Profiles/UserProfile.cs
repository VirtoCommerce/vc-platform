using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Model.Profiles
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

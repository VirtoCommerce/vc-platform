using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings
{
    public class SettingsSearchCriteria : SearchCriteriaBase
    {
        public string ModuleId { get; set; }
        public bool? IsHidden { get; set; }
    }
}

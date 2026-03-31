using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Search criteria for the Settings V2 schema endpoint
    /// </summary>
    public class SettingsPropertySearchCriteria : SearchCriteriaBase
    {
        /// <summary>
        /// Filter by module ID
        /// </summary>
        public string ModuleId { get; set; }
    }
}

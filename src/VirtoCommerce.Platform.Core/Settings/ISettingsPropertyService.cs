using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Settings V2 service that separates schema from values
    /// and supports Global / Tenant scoping.
    /// </summary>
    public interface ISettingsPropertyService
    {
        /// <summary>
        /// Returns a flat array of property schemas (metadata only, no values).
        /// Client builds the group tree from GroupName.
        /// </summary>
        Task<IReadOnlyList<SettingPropertySchema>> GetSchemaAsync(
            SettingsPropertySearchCriteria criteria,
            string tenantType = null);

        /// <summary>
        /// Returns property values as a flat { Name: Value } dictionary.
        /// When modifiedOnly is true, only returns values that differ from DefaultValue.
        /// </summary>
        Task<Dictionary<string, object>> GetValuesAsync(
            string tenantType = null,
            string tenantId = null,
            bool modifiedOnly = false);

        /// <summary>
        /// Updates property values from a flat { Name: Value } dictionary.
        /// Only settings included in the dictionary are updated.
        /// </summary>
        Task SaveValuesAsync(
            Dictionary<string, object> values,
            string tenantType = null,
            string tenantId = null);
    }
}

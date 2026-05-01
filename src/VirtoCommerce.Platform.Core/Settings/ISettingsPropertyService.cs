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
        /// When <paramref name="modifiedOnly"/> is true, only returns values that differ from DefaultValue.
        /// When <paramref name="moduleId"/> is non-empty, the result is narrowed to settings owned by that module.
        /// This mirrors the same filter on <see cref="GetSchemaAsync"/> so callers that only want one
        /// module's read-and-use values can do a single round-trip without a follow-up schema lookup.
        /// </summary>
        Task<Dictionary<string, object>> GetValuesAsync(
            string tenantType = null,
            string tenantId = null,
            bool modifiedOnly = false,
            string moduleId = null);

        /// <summary>
        /// Updates property values from a flat { Name: Value } dictionary.
        /// When replaceAll is false (default), only settings in the dictionary are updated (merge/patch).
        /// When replaceAll is true, the dictionary is the complete set of desired modifications —
        /// any currently-modified setting NOT in the dictionary is reset to its default value.
        /// </summary>
        Task SaveValuesAsync(
            Dictionary<string, object> values,
            string tenantType = null,
            string tenantId = null,
            bool replaceAll = false);
    }
}

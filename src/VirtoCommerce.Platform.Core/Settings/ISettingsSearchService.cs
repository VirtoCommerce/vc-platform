using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Settings
{
    public interface ISettingsSearchService : ISearchService<SettingsSearchCriteria, SettingsSearchResult, ObjectSettingEntry>
    {
    }
}

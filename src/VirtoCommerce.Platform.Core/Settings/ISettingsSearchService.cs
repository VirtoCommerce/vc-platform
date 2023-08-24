using System;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Settings
{
    public interface ISettingsSearchService : ISearchService<SettingsSearchCriteria, SettingsSearchResult, ObjectSettingEntry>
    {
        [Obsolete("Use SearchAsync()", DiagnosticId = "VC0005", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        Task<GenericSearchResult<ObjectSettingEntry>> SearchSettingsAsync(SettingsSearchCriteria searchCriteria);
    }
}

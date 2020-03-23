using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings
{
    public interface ISettingsSearchService
    {
        Task<GenericSearchResult<ObjectSettingEntry>> SearchSettingsAsync(SettingsSearchCriteria searchCriteria);
    }
}

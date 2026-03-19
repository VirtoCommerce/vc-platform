using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class SettingsSearchService : ISettingsSearchService
    {
        private readonly ISettingsManager _settingsManager;

        public SettingsSearchService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task<SettingsSearchResult> SearchAsync(SettingsSearchCriteria criteria, bool clone = true)
        {
            var result = AbstractTypeFactory<SettingsSearchResult>.TryCreateInstance();

            var query = _settingsManager.AllRegisteredSettings.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.ModuleId))
            {
                query = query.Where(x => x.ModuleId == criteria.ModuleId);
            }

            if (criteria.IsHidden != null)
            {
                query = query.Where(x => x.IsHidden == criteria.IsHidden);
            }

            var sortInfos = criteria.SortInfos;
            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
            }
            query = query.OrderBySortInfos(sortInfos);
            result.TotalCount = query.Count();
            var names = query.Skip(criteria.Skip).Take(criteria.Take).Select(x => x.Name).ToList();

            var settings = await _settingsManager.GetObjectSettingsAsync(names.ToArray());
            result.Results = settings.OrderBy(x => names.IndexOf(x.Name))
                                       .ToList();
            return result;
        }
    }
}

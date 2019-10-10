using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public interface IChangeLogSearchService
    {
        Task<ChangeLogSearchResult> SearchAsync(ChangeLogSearchCriteria criteria);
    }
}

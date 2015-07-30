using Hangfire;
using VirtoCommerce.Domain.Search.Model;

namespace VirtoCommerce.SearchModule.Web.BackgroundJobs
{
    public class SearchIndexJobsScheduler
    {
        private readonly ISearchConnection _searchConnection;

        public SearchIndexJobsScheduler(ISearchConnection searchConnection)
        {
            _searchConnection = searchConnection;
        }

        public void ScheduleJobs()
        {
            RecurringJob.AddOrUpdate<SearchIndexJobs>("CatalogIndexJob", x => x.Process(_searchConnection.Scope, CatalogIndexedSearchCriteria.DocType, false), "*/1 * * * *");
        }

        public string ScheduleRebuildIndex()
        {
            var jobId = BackgroundJob.Enqueue<SearchIndexJobs>(x => x.Process(_searchConnection.Scope, CatalogIndexedSearchCriteria.DocType, true));
            return jobId;
        }
    }
}

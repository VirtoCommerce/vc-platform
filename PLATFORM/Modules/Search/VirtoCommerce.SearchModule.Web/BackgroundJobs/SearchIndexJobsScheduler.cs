using Hangfire;
using VirtoCommerce.Domain.Search;

namespace VirtoCommerce.InventoryModule.Web.BackgroundJobs
{
    public class SearchIndexJobsScheduler
    {
        private readonly ISearchConnection _searchConnection;

        public SearchIndexJobsScheduler(ISearchConnection searchConnection)
        {
            _searchConnection = searchConnection;
        }

        public void SheduleJobs()
        {
             RecurringJob.AddOrUpdate<SearchIndexJobs>("catalogIndexJob", x => x.Process(_searchConnection.Scope, "catalogitem"), Cron.Minutely);
        }
    }
}

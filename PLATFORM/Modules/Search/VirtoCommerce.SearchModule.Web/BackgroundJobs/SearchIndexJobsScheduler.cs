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
             RecurringJob.AddOrUpdate<SearchIndexJobs>("813dea57-494e-434a-b4b4-6027e4d76f8f", x => x.Process(_searchConnection.Scope), Cron.Minutely);
        }
    }
}

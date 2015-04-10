using Hangfire;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.MerchandisingModule.Web.BackgroundJobs
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
            RecurringJob.AddOrUpdate<SearchIndexJobs>("a7d5a1b7-2ec1-4190-8a9f-d4ac8079496d", x => x.Prepare(_searchConnection.Scope), Cron.Minutely);
            RecurringJob.AddOrUpdate<SearchIndexJobs>("813dea57-494e-434a-b4b4-6027e4d76f8f", x => x.Process(_searchConnection.Scope), Cron.Minutely);
        }
    }
}

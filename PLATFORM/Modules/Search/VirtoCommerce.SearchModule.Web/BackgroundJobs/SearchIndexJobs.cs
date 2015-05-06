using Hangfire;
using VirtoCommerce.Domain.Search.Services;

namespace VirtoCommerce.SearchModule.Web.BackgroundJobs
{
    public class SearchIndexJobs
    {
        private readonly ISearchIndexController _controller;

        public SearchIndexJobs(ISearchIndexController controller)
        {
            _controller = controller;
        }

        [DisableConcurrentExecution(int.MaxValue)]
        public void Process(string scope, string documentType)
        {
            _controller.Process(scope, documentType, false);
        }
    }
}

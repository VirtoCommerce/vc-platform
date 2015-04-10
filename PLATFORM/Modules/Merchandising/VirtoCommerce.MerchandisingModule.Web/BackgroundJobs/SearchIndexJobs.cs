using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.MerchandisingModule.Web.BackgroundJobs
{
    public class SearchIndexJobs
    {
        private readonly ISearchIndexController _controller;

        public SearchIndexJobs(ISearchIndexController controller)
        {
            _controller = controller;
        }

        public void Prepare(string scope)
        {
            _controller.Prepare(scope);
        }

        public void Process(string scope)
        {
            _controller.Process(scope);
        }
    }
}

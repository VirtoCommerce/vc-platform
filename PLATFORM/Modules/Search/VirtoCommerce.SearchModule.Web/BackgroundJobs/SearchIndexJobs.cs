using VirtoCommerce.Domain.Search.Services;
namespace VirtoCommerce.InventoryModule.Web.BackgroundJobs
{
    public class SearchIndexJobs
    {
        private readonly ISearchIndexController _controller;

        public SearchIndexJobs(ISearchIndexController controller)
        {
            _controller = controller;
        }

        public void Process(string scope)
        {
            _controller.Process(scope);
        }
    }
}

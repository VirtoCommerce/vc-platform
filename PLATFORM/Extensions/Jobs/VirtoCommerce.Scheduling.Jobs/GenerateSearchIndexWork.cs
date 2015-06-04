using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Scheduling.Jobs
{
    public class GenerateSearchIndexWork : IJobActivity
    {
        private readonly ISearchIndexController _controller;
        public GenerateSearchIndexWork(ISearchIndexController controller)
        {
            _controller = controller;
        }

        public void Execute(IJobContext context)
        {
            var scope = context.Parameters != null && context.Parameters.ContainsKey("scope") 
                ? context.Parameters["scope"] : "default";
            _controller.Prepare(scope);
        }
    }
}

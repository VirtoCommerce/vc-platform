using System;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Scheduling.Jobs
{
    public class ProcessSearchIndexWork : IJobActivity
    {
        private readonly ISearchIndexController _controller;
        public ProcessSearchIndexWork(ISearchIndexController controller)
        {
            _controller = controller;
        }

        public void Execute(IJobContext context)
        {
            var scope = context.Parameters != null && context.Parameters.ContainsKey("scope")
                ? context.Parameters["scope"] : "default";
            _controller.Process(scope);
        }
    }
}

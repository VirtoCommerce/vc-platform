using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.Extensions;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public sealed class ProcessPlatformRestarter : IPlatformRestarter
    {
        public ProcessPlatformRestarter(IHostApplicationLifetime applicationLifetime)
        {
            ApplicationLifetime = applicationLifetime;
        }

        private IHostApplicationLifetime ApplicationLifetime { get; }

        public void Restart()
        {
            // stop the application, it will be auto restarted by IIS or docker container
            ApplicationLifetime.StopApplication();
        }
    }
}

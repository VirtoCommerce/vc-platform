using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace VirtoCommerce.Platform.MigrationScripter
{
    /// <summary>
    /// Minimal <see cref="IHostEnvironment"/> passed to module <c>Initialize</c> for modules that
    /// implement <c>IHasHostEnvironment</c>. We are not running a host, so this only carries basic values.
    /// </summary>
    internal sealed class SimpleHostEnvironment : IHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
    }
}

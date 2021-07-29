using Microsoft.Extensions.Hosting;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Used to set HostEnvironment for modules during initialization.
    /// In order to acces to HostEnvironment in a module you have to implement this interfaces for your IModule class
    /// </summary>
    public interface IHasHostEnvironment
    {
        public IHostEnvironment HostEnvironment { get; set; }
    }
}

using Microsoft.Extensions.Hosting;
namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IHasHostEnvironment
    {
        public IHostEnvironment HostEnvironment { get; set; }
    }
}

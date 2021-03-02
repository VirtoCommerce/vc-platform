using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public interface IHasConfiguration
    {
        IConfiguration Configuration { get; set; }
    }
}

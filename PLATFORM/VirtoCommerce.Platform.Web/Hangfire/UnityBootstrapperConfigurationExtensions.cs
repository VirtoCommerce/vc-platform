using Hangfire;
using Microsoft.Practices.Unity;

namespace VirtoCommerce.Platform.Web.Hangfire
{
    public static class UnityBootstrapperConfigurationExtensions
    {
        public static void UseUnityActivator(this IBootstrapperConfiguration configuration, IUnityContainer container)
        {
            configuration.UseActivator(new UnityJobActivator(container));
        }
    }
}

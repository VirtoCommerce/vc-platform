using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace VirtoCommerce.Bootstrapper.Main.Infrastructure.Extensions
{
    public static class EngineExtensions
    {
        public static async Task PlanAsync(this Engine engine, LaunchAction action)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => engine.Plan(action), DispatcherPriority.Background);
        }
    }
}
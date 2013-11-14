using Microsoft.Practices.Prism.Commands;

using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public interface IMainViewModel : IWizard
    {
        DelegateCommand<object> CancelCommand { get; }

        bool Cancelled { get; }

        bool Finished { get; set; }

        void Finish();

        void Shutdown(int exitCode);
    }
}
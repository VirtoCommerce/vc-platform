using VirtoCommerce.Foundation.Search;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces
{
    public interface IConfirmationStepViewModel : IWizardStep
    {
        string ProjectLocation { get; set; }

        string SqlServerAuthentication { get; set; }

        string DatabaseConnectionString { get; set; }

        SearchConnection SearchConnection { get; set; }
    }
}
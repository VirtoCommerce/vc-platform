using System.ComponentModel;
using System.Threading;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces
{
	public interface IConfigureStep : IWizardStep, IDataErrorInfo
	{
		string Action { get; }

		string Message { get; }

		void Configure(CancellationToken ct);

		void Cancel();

		OperationResult? Result { get; }
		IConfigurationViewModel Configuration { get; set; }
	}
}
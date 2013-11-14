using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces
{
	public interface IDatabaseSettingsStepViewModel : IConfigureStep
	{
		string Server { get; set; }

		IEnumerable<string> AvailableServers { get; }

		string DatabaseName { get; set; }

		SqlServerAuthentication ServerAuthentication { get; set; }

		IEnumerable<SqlServerAuthentication> ServerAuthenticationMethods { get; }

		string Login { get; set; }

		string Password { get; set; }

		bool InstallSamples { get; set; }

		DelegateCommand TestConnectionCommand { get; }

		bool IsNotTestedConnection { get; set; }

		bool IsConnectionValid { get; set; }
	}
}
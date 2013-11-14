using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels
{
	public interface IConfigurationViewModel : IViewModel
	{
		ObservableCollection<KeyValuePair<string, string>> Steps { get; }

		bool IsValid { get; set; }

		string Message { get; set; }

		OperationResult Result { get; set; }

		DelegateCommand<object> CancelCommand { get; }
		CancellationTokenSource CancellationSource { set; }

		void Finish();
	}
}
using System;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Security.Model;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
	public interface ILoginViewModel : IViewModel
	{
		DelegateCommand LoginCommand { get; }
        string Error { get; set; }
        Login CurrentUser { get; }
        event EventHandler LogonViewRequestedEvent;
	}
}

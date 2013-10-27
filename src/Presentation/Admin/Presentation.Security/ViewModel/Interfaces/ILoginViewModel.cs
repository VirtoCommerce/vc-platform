using System;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
	public interface ILoginViewModel : IViewModel
	{
		DelegateCommand<object> LoginCommand { get; }
		bool AuthProgress { get; }
		string UserName { get; set; }
		string Password { get; set; }
        string Error { get; set; }
        string BaseUrl { get; set; }
        string CurrentUserName { get; set; }
        bool IsAnimation { get; }
        event EventHandler LogonViewRequestedEvent;
	    //bool RememberMe { get; set; }
	}
}

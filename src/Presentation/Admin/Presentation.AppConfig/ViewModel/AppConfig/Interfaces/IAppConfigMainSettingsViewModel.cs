using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces
{
	public interface IAppConfigMainSettingsViewModel: IViewModel
	{
		List<ItemTypeHomeTab> SubItems { get; }
	}
}

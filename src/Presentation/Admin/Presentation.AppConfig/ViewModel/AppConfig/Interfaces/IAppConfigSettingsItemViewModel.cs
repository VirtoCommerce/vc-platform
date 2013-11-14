using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces
{
	public interface IAppConfigSettingsItemViewModel : IViewModel
	{
		string Id { get; set; }
		string Name { get; set; }
		string Value { get; set; }
		bool IsSystem { get; set; }
	}
}

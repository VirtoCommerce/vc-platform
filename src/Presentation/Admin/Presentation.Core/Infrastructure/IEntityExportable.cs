using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public interface IEntityExportable
	{
		DelegateCommand ListExportCommand { get; }
	}
}

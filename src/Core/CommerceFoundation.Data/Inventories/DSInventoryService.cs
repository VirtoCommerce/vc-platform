using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;
using System.Data.Services.Providers;

namespace VirtoCommerce.Foundation.Data.Inventories
{
    [JsonSupportBehavior]
	public class DSInventoryService : DServiceBase<EFInventoryRepository>
	{
	}
}

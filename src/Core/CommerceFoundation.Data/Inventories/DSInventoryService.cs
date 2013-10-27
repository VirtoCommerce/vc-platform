using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Data.Infrastructure;

namespace VirtoCommerce.Foundation.Data.Inventories
{
    [JsonSupportBehavior]
	public class DSInventoryService : DServiceBase<EFInventoryRepository>
	{
		protected override EFInventoryRepository CreateDataSource()
		{
			return new EFInventoryRepository(new InventoryEntityFactory());
		}
	}
}

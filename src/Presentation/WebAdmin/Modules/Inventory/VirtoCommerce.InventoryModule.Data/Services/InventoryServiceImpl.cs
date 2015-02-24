using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.InventoryModule.Data.Converters;
using VirtoCommerce.InventoryModule.Data.Repositories;
using coreModel = VirtoCommerce.Domain.Inventory.Model;
using foundationModel = VirtoCommerce.Foundation.Inventories.Model;

namespace VirtoCommerce.InventoryModule.Data.Services
{
	public class InventoryServiceImpl : ServiceBase, IInventoryService
	{
		private readonly Func<IFoundationInventoryRepository> _repositoryFactory;
		public InventoryServiceImpl(Func<IFoundationInventoryRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#region IInventoryService Members

		public IEnumerable<coreModel.InventoryInfo> GetProductsInventoryInfos(IEnumerable<string> productIds)
		{
			var retVal = new List<coreModel.InventoryInfo>();
			using (var repository = _repositoryFactory())
			{
				var entities = repository.GetProductsInventories(productIds.ToArray());
				retVal.AddRange(entities.Select(x => x.ToCoreModel()));
			}
			return retVal;
		}

		public void UpsertInventories(IEnumerable<coreModel.InventoryInfo> inventoryInfos)
		{
			using (var repository = _repositoryFactory())
			{
				var sourceEntities = inventoryInfos.Select(x => x.ToFoundation()).ToList();
				var changedProductIds = inventoryInfos.Select(x => x.ProductId).ToArray();
				var targetEntities = repository.GetProductsInventories(changedProductIds).ToList();

				var targetCollection = new ObservableCollection<foundationModel.Inventory>(targetEntities);
				targetCollection.ObserveCollection(x => repository.Add(x), null);
				var inventoryComparer = AnonymousComparer.Create((foundationModel.Inventory x) => x.FulfillmentCenterId + "-" + x.Sku);
				sourceEntities.Patch(targetCollection, inventoryComparer, (source, target) => source.Patch(target));

				CommitChanges(repository);
			}
		}

	
		public coreModel.InventoryInfo UpsertInventory(coreModel.InventoryInfo inventoryInfo)
		{
			if (inventoryInfo == null)
				throw new ArgumentNullException("inventoryInfo");

			coreModel.InventoryInfo retVal = null;
			using (var repository = _repositoryFactory())
			{
				var sourceInventory = inventoryInfo.ToFoundation();

				var alreadyExistInventories =  repository.GetProductsInventories(new string[]  { inventoryInfo.ProductId });
				var targetInventory = alreadyExistInventories.FirstOrDefault(x => x.FulfillmentCenterId == sourceInventory.FulfillmentCenterId);
				if (targetInventory == null)
				{
					targetInventory = sourceInventory;
					repository.Add(targetInventory);
				}
				else
				{
					sourceInventory.Patch(targetInventory);
				}
				CommitChanges(repository);
				retVal = targetInventory.ToCoreModel();
			}
			return retVal;
		}

		#endregion
	}
}

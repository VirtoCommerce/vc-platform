using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.StoreModule.Data.Repositories;
using VirtoCommerce.StoreModule.Data.Converters;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Domain.Commerce.Services;

namespace VirtoCommerce.StoreModule.Data.Services
{
	public class StoreServiceImpl : ServiceBase, IStoreService
	{
		private readonly Func<IStoreRepository> _repositoryFactory;
		private readonly ICommerceService _commerceService;
		public StoreServiceImpl(Func<IStoreRepository> repositoryFactory, ICommerceService commerceService)
		{
			_repositoryFactory = repositoryFactory;
			_commerceService = commerceService;
		}

		#region IStoreService Members

		public coreModel.Store GetById(string id)
		{
			coreModel.Store retVal = null;
			using (var repository = _repositoryFactory())
			{
				var entity = repository.GetStoreById(id);
				if (entity != null)
				{
					retVal = entity.ToCoreModel();
				}
				var fulfillmentCenters = _commerceService.GetAllFulfillmentCenters();
				retVal.ReturnsFulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.ReturnsFulfillmentCenterId);
				retVal.FulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.FulfillmentCenterId);
			}

			return retVal;
		}

		public coreModel.Store Create(coreModel.Store store)
		{
			var entity = store.ToFoundation();
			coreModel.Store retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
			}
			retVal = GetById(store.Id);
			return retVal;
		}

		public void Update(coreModel.Store[] stores)
		{
			using (var repository = _repositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var store in stores)
				{
					var sourceEntity = store.ToFoundation();
					var targetEntity = repository.GetStoreById(store.Id);
					if (targetEntity == null)
					{
						throw new NullReferenceException("targetEntity");
					}

					changeTracker.Attach(targetEntity);
					sourceEntity.Patch(targetEntity);
				}
				CommitChanges(repository);
			}
		}

		public void Delete(string[] ids)
		{
			using (var repository = _repositoryFactory())
			{
				foreach (var id in ids)
				{
					var entity = repository.GetStoreById(id);
					repository.Remove(entity);
				}
				CommitChanges(repository);
			}
		}

		public IEnumerable<coreModel.Store> GetStoreList()
		{
			var retVal = new List<coreModel.Store>();
			using (var repository = _repositoryFactory())
			{
				foreach (var storeId in repository.Stores.Select(x => x.Id).ToArray())
				{
					var entity = repository.GetStoreById(storeId);
					retVal.Add(entity.ToCoreModel());
				}
			}
			return retVal;
		}

		#endregion
	}
}

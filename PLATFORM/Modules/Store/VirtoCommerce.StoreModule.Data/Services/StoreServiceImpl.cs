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
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Shipping.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Payment.Services;

namespace VirtoCommerce.StoreModule.Data.Services
{
	public class StoreServiceImpl : ServiceBase, IStoreService
	{
		private readonly Func<IStoreRepository> _repositoryFactory;
		private readonly ICommerceService _commerceService;
		private readonly ISettingsManager _settingManager;
		private readonly IShippingService _shippingService;
		private readonly IPaymentMethodsService _paymentService;

		public StoreServiceImpl(Func<IStoreRepository> repositoryFactory, ICommerceService commerceService, ISettingsManager settingManager, IShippingService shippingService, IPaymentMethodsService paymentService)
		{
			_repositoryFactory = repositoryFactory;
			_commerceService = commerceService;
			_settingManager = settingManager;
			_shippingService = shippingService;
			_paymentService = paymentService;
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
					//Load original typed shipping method and populate it  personalized information from db
					retVal = entity.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods());

					var fulfillmentCenters = _commerceService.GetAllFulfillmentCenters();
					retVal.ReturnsFulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.ReturnsFulfillmentCenterId);
					retVal.FulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.FulfillmentCenterId);
					retVal.SeoInfos = _commerceService.GetSeoKeywordsForEntity(id).Select(x => x.ToCoreModel()).ToList();

					LoadObjectSettings(_settingManager, retVal);
				}

			}

			return retVal;
		}

		public coreModel.Store Create(coreModel.Store store)
		{
			var entity = store.ToDataModel();
			coreModel.Store retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
			}

			SaveObjectSettings(_settingManager, store);

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
					var sourceEntity = store.ToDataModel();
					var targetEntity = repository.GetStoreById(store.Id);
					if (targetEntity == null)
					{
						throw new NullReferenceException("targetEntity");
					}

					changeTracker.Attach(targetEntity);
					sourceEntity.Patch(targetEntity);

					SaveObjectSettings(_settingManager, store);
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
					var store = GetById(id);
					RemoveObjectSettings(_settingManager, store);

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
					var store = GetById(storeId);
					retVal.Add(store);
				}
			}
			return retVal;
		}

		#endregion

	}
}

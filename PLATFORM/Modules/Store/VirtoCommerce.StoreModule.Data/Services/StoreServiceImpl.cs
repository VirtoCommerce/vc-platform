using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Caching;
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
using VirtoCommerce.Domain.Payment.Services;
using System.Collections.ObjectModel;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.StoreModule.Data.Services
{
	public class StoreServiceImpl : ServiceBase, IStoreService
	{
		private readonly Func<IStoreRepository> _repositoryFactory;
		private readonly ICommerceService _commerceService;
		private readonly ISettingsManager _settingManager;
		private readonly IShippingService _shippingService;
		private readonly IPaymentMethodsService _paymentService;
	    private readonly CacheManager _cacheManager;
        private readonly CacheKey _storesCacheKey;

        public StoreServiceImpl(Func<IStoreRepository> repositoryFactory, CacheManager cacheManager, ICommerceService commerceService, ISettingsManager settingManager, IShippingService shippingService, IPaymentMethodsService paymentService)
		{
		    _cacheManager = cacheManager;
			_repositoryFactory = repositoryFactory;
			_commerceService = commerceService;
			_settingManager = settingManager;
			_shippingService = shippingService;
			_paymentService = paymentService;
            _storesCacheKey = CacheKey.Create("Virto.Core.Stores");
		}

		#region IStoreService Members

	    public coreModel.Store GetById(string id)
	    {
            var stores = this.GetStoreList();
            return stores.FirstOrDefault(s => s.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
	    }

		public coreModel.Store Create(coreModel.Store store)
		{
			var dbStore = store.ToDataModel();
		    using (var repository = _repositoryFactory())
			{
				repository.Add(dbStore);
				CommitChanges(repository);
			}

			//Need add seo separately
			if (store.SeoInfos != null)
			{
				foreach (var seoInfo in store.SeoInfos)
				{
					seoInfo.ObjectId = dbStore.Id;
					seoInfo.ObjectType = typeof(coreModel.Store).Name;
					_commerceService.UpsertSeo(seoInfo);
				}
			}

			//Deep save settings
			SaveObjectSettings(_settingManager, store);

            // clean up cache
            _cacheManager.Remove(this._storesCacheKey);

			var retVal = this.GetById(store.Id);
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

					//Patch SeoInfo  separately
					if (store.SeoInfos != null)
					{
						foreach(var seoInfo in store.SeoInfos)
						{
							seoInfo.ObjectId = store.Id;
							seoInfo.ObjectType = typeof(coreModel.Store).Name;
						}
						var seoInfos = new ObservableCollection<SeoInfo>(_commerceService.GetObjectsSeo(new[] { store.Id }));
						seoInfos.ObserveCollection(x => _commerceService.UpsertSeo(x), x => _commerceService.DeleteSeo(new[] { x.Id }));
						store.SeoInfos.Patch(seoInfos, (source, target) => _commerceService.UpsertSeo(source));
					}
				}
				CommitChanges(repository);
			}

            // clean up cache
            _cacheManager.Remove(this._storesCacheKey);
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

            // clean up cache
            _cacheManager.Remove(this._storesCacheKey);
		}

        public IEnumerable<coreModel.Store> GetStoreList()
        {
            var stores = _cacheManager.Get(this._storesCacheKey, this.LoadStores);
            return stores;
        }
		#endregion

        #region private methods
	    private coreModel.Store[] LoadStores()
	    {
            var stores = new List<coreModel.Store>();
	        using (var repository = _repositoryFactory())
	        {
	            var entities = repository.GetAllStores();
	            if (entities.Any())
	            {
	                var fulfillmentCenters = _commerceService.GetAllFulfillmentCenters();
	                var seo = _commerceService.GetObjectsSeo(entities.Select(x => x.Id).ToArray()).ToList();

	                foreach (var entity in entities)
	                {
	                    var store = entity.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods());
	                    store.ReturnsFulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.ReturnsFulfillmentCenterId);
	                    store.FulfillmentCenter = fulfillmentCenters.FirstOrDefault(x => x.Id == entity.FulfillmentCenterId);
	                    store.SeoInfos = seo != null ? seo.Where(x => x.ObjectId == entity.Id).ToList() : null;
	                    LoadObjectSettings(_settingManager, store);
	                    stores.Add(store);
	                }
	            }
	        }

	        return stores.ToArray();
	    }
	    #endregion
    }
}
